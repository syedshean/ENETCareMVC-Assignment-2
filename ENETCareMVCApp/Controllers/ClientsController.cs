using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ENETCareMVCApp.Models;
using Microsoft.AspNet.Identity;
using ENETCareMVCApp.Repositories;

namespace ENETCareMVCApp.Controllers
{
    public class ClientsController : Controller
    {
        private DBContext db = new DBContext();
        IClientRepository repository;

        public ClientsController()
        {
            db = new DBContext();
            repository = new ClientRepository();
        }

        public ClientsController(IClientRepository repository)
        {
            this.repository = repository;
        }

        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Index()
        {
            string logedUser = User.Identity.Name;
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            var clients = db.Clients.Include(c => c.District).Where(i=>i.DistrictID==districtID);
            return View(clients.ToList());
        }

        //// GET: Clients/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Client client = db.Clients.Find(id);
        //    if (client == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(client);
        //}
        
        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Create()
        {
            District userDistrict = GetUserDistrict();
            Client aClientModel = new Client
            {
                District = userDistrict,
                DistrictID = userDistrict.DistrictID,
            };
            return View(aClientModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Create([Bind(Include = "ClientID,ClientName,Address,DistrictID")] Client client)
        {
            bool isExist = repository.IsUserNameExits(client.ClientName);
            if (ModelState.IsValid && (isExist==false))
            {
                Client aClient = repository.AddClients(client);
                return RedirectToAction("Index", new { id = aClient.ClientID });
            }
            // If insert fails
            District userDistrict = GetUserDistrict();
            client.DistrictID = userDistrict.DistrictID;
            client.District = userDistrict;
            if (isExist == true)
                ViewBag.Message = "This client already existes";
            else
                ViewBag.Message = "";
            //ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", client.DistrictID);
            return View(client);
        }

        //[NonAction]
        //private bool IsUserNameExits(string clientName)
        //{
        //    Client client;
        //    using (var db = new DBContext())
        //    {
        //        client = db.Clients.Where(i => i.ClientName == clientName).FirstOrDefault();  
        //    }
        //    if (client == null)
        //        return false;
        //    else return true;
        //}

        // GET: Clients/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Client client = db.Clients.Find(id);
        //    if (client == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", client.DistrictID);
        //    return View(client);
        //}

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ClientID,ClientName,Address,DistrictID")] Client client)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(client).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", client.DistrictID);
        //    return View(client);
        //}

        // GET: Clients/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Client client = db.Clients.Find(id);
        //    if (client == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(client);
        //}

        // POST: Clients/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Client client = db.Clients.Find(id);
        //    db.Clients.Remove(client);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Authorize(Roles = "SiteEngineer")]
        // ClientList with Interventions
        public ActionResult ClientListWithIntervention()
        {
            List<Intervention> interventions;
            List<Client> aClientList;
            List<ClientWithInterventionModel> aClientWithInterventionList = new List<ClientWithInterventionModel>();
            using (var db = new DBContext())
            {
                interventions = db.Interventions.Where(i => i.InterventionState != InterventionState.Cancelled).ToList();
                aClientList = GetClientListByDistrict(GetUserDistrict().DistrictID);

                foreach (var intervention in interventions)
                {
                    var client = aClientList.Where(c => c.ClientID == intervention.ClientID).FirstOrDefault();
                    if(client != null)
                    {
                        ClientWithInterventionModel aClientWithIntervention = new ClientWithInterventionModel();
                        aClientWithIntervention.aClient = client;
                        aClientWithIntervention.DistrictName = GetUserDistrict().DistrictName;
                        aClientWithIntervention.InterventionID = intervention.InterventionID;
                        aClientWithIntervention.InterventionState = intervention.InterventionState.ToString();
                        aClientWithIntervention.InterventionTypeName = GetInterventionTypeByInterventionTypeID(intervention.InterventionTypeID).InterventionTypeName;
                        aClientWithIntervention.UserName = GetUserDetailsByUserID(intervention.UserID).UserName;
                        aClientWithInterventionList.Add(aClientWithIntervention);
                    }
                }
            }
            return View(aClientWithInterventionList);
        }

        //Returns the district of logged in user 
        [NonAction]
        private District GetUserDistrict()
        {
            string logedUser = User.Identity.Name;
            User currentUser = (db.Users.Where(u => u.LoginName == logedUser)).First();
            if (currentUser == null)
            {
                RedirectToAction("Index", "Home");
            }
            District userDistrict = currentUser.District;
            return userDistrict;
        }

        [NonAction]
        public List<Client> GetClientListByDistrict(int districtID)
        {
            List<Client> aClientList = new List<Client>();
            using (var db = new DBContext())
            {
                aClientList = (from c in db.Clients
                               where c.District.DistrictID == districtID
                               select c).ToList();

            }
            return aClientList;
        }

        [NonAction]
        public List<Client> GetClientList()
        {
            List<Client> aClientList = new List<Client>();
            using (var db = new DBContext())
            {
                aClientList = (from c in db.Clients
                               select c).ToList();

            }
            return aClientList;
        }

        [NonAction]
        public string GetClientNameByClientID(int clientID)
        {

            string clientName;
            using (var db = new DBContext())
            {
                clientName = (from c in db.Clients
                              where c.ClientID == clientID
                              select c.ClientName).ToString();

            }
            return clientName;
        }

        [NonAction]
        public InterventionType GetInterventionTypeByInterventionTypeID(int interventionTypeID)
        {
            InterventionType anInterventionType;
            using (var db = new DBContext())
            {
                anInterventionType = (from i in db.InterventionTypes
                                      where i.InterventionTypeID == interventionTypeID
                                      select i).First();
            }
            return anInterventionType;
        }

        [NonAction]
        public User GetUserDetailsByUserID(int userID)
        {
            User anUser;
            using (var db = new DBContext())
            {
                anUser = (from u in db.Users
                          where u.UserID == userID
                          select u).First();
            }
            return anUser;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
