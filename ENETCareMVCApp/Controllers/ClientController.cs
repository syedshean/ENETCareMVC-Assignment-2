using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ENETCareMVCApp.DAL;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Controllers
{
    public class ClientController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Client
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Client/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Client/Create
        public ActionResult Create()
        {
            // note getting loginname null
            //string loginName = ViewBag.UserName;
            //using (DBContext dbContext = new DBContext())
            //{
            //    var district = from u in dbContext.Users
            //                   where u.LoginName.Equals(loginName)
            //                   select u.District;

            //    District aDistrict = (District)district;
            //    ViewData["districtName"] = aDistrict.DistrictName;
            //}
            return View();
        }

        // POST: Client/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientID,ClientName,Address,DistrictID")] Client client)
        {
            string loginName = ViewBag.UserName;
            using (DBContext dbContext = new DBContext())
            {
                var district = from u in dbContext.Users
                               where u.LoginName.Equals(loginName)
                               select u.District;

                District aDistrict = (District)district;
                client.District = aDistrict;
                client.District.DistrictID = aDistrict.DistrictID;
            }
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientID,ClientName,Address")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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

        //[NonAction]
        //public bool IsUserNameExist(string clientName)
        //{
        //    bool isExist = false;
        //    connectionString = aDatabaseConfig.Setup("ENETCareDatabase");
        //    using (SqlConnection connection = new SqlConnection())
        //    {
        //        connection.ConnectionString = connectionString;
        //        string query = "SELECT * FROM Client WHERE ClientName=@clientName";

        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.Add(new SqlParameter("clientName", clientName));

        //        try
        //        {
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();
        //            while (reader.Read())
        //            {
        //                isExist = true;
        //            }
        //        }
        //        catch { }
        //    }
        //    return isExist;
        //}
    }
}
