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
            return View(client);
        }

        [Authorize(Roles = "SiteEngineer")]
        // ClientList with Interventions
        public ActionResult ClientListWithIntervention()
        {
            List<Intervention> interventions;
            List<Client> aClientList;
            List<ClientWithInterventionModel> aClientWithInterventionList = new List<ClientWithInterventionModel>();
            using (var db = new DBContext())
            {
                interventions = repository.GetInterventionList();
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
            User currentUser = (db.Users.Where(u => u.LoginName == logedUser)).FirstOrDefault();
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
            return repository.GetClientListByDistrict(districtID);
        }

        //[NonAction]
        //public List<Client> GetClientList()
        //{
        //    List<Client> aClientList = new List<Client>();
        //    using (var db = new DBContext())
        //    {
        //        aClientList = (from c in db.Clients
        //                       select c).ToList();

        //    }
        //    return aClientList;
        //}

        //[NonAction]
        //public string GetClientNameByClientID(int clientID)
        //{

        //    string clientName;
        //    using (var db = new DBContext())
        //    {
        //        clientName = (from c in db.Clients
        //                      where c.ClientID == clientID
        //                      select c.ClientName).ToString();

        //    }
        //    return clientName;
        //}

        [NonAction]
        public InterventionType GetInterventionTypeByInterventionTypeID(int interventionTypeID)
        {
            InterventionType anInterventionType;
            using (var db = new DBContext())
            {
                anInterventionType = repository.GetInterventionTypeByInterventionTypeID(interventionTypeID);
            }
            return anInterventionType;
        }

        [NonAction]
        public User GetUserDetailsByUserID(int userID)
        {
            //User anUser;
            //using (var db = new DBContext())
            //{
            //    anUser = (from u in db.Users
            //              where u.UserID == userID
            //              select u).First();
            //}
            //return anUser;
            return repository.GetUserDetails(userID);
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
