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
    public class InterventionsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Interventions
        public ActionResult Index()
        {
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType);
            return View(interventions.ToList());
        }

        public ActionResult PreviousInterventionList()
        {
            string logedUser = User.Identity.Name;
            //int userID = db.Interventions.Where(i=>i.User.LoginName==logedUser).FirstOrDefault().UserID;
            int userID = Int32.Parse(GetUserIDByUserName(logedUser));
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Where(i=>i.UserID== userID);
            return View(interventions.ToList());
        }

        // GET: Interventions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intervention intervention = db.Interventions.Find(id);
            if (intervention == null)
            {
                return HttpNotFound();
            }
            return View(intervention);
        }

        // GET: Interventions/Create
        public ActionResult Create()
        {
            string logedUser = User.Identity.Name;
            User currentUser = (db.Users.Where(u => u.LoginName == logedUser)).First();
            Intervention aInterventionModel = new Intervention
            {
                User = currentUser,
                UserID = currentUser.UserID,
            };
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName");
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName");
            return View(aInterventionModel);
        }

        // POST: Interventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,UserID,InterventionTypeID")] Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                db.Interventions.Add(intervention);
                db.SaveChanges();
                return RedirectToAction("PreviousInterventionList");
            }
            string logedUser = User.Identity.Name;
            User currentUser = (db.Users.Where(u => u.LoginName == logedUser)).First();
            intervention.UserID = currentUser.UserID;
            intervention.User = currentUser;

            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        // GET: Interventions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intervention intervention = db.Interventions.Find(id);
            if (intervention == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        // POST: Interventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,InterventionTypeID")] Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                db.Interventions.Attach(intervention);
                db.Entry(intervention).Property(i => i.InterventionState).IsModified = true;
                //db.Entry(intervention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        // GET: Interventions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Intervention intervention = db.Interventions.Find(id);
            if (intervention == null)
            {
                return HttpNotFound();
            }
            return View(intervention);
        }

        // POST: Interventions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Intervention intervention = db.Interventions.Find(id);
            db.Interventions.Remove(intervention);
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
        public string GetUserIDByUserName(string userName)
        {

            string userID;
            using (var db = new DBContext())
            {
                userID = (from u in db.Users
                          where u.LoginName == userName
                          select u.UserID).FirstOrDefault().ToString();

            }
            return userID;
        }
    }
}
