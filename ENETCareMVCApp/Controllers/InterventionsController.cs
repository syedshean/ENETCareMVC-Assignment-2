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
            return View(db.Interventions.ToList());
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
            return View();
        }

        // POST: Interventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,Notes,RemainingLife,LastEditDate")] Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                db.Interventions.Add(intervention);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            return View(intervention);
        }

        // POST: Interventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,Notes,RemainingLife,LastEditDate")] Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                db.Entry(intervention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
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
        public List<Intervention> GetInterventionListByClient(int clientID)
        {
            List<Intervention> anInterventionList = new List<Intervention>();
            using (var db = new DBContext())
            {
                anInterventionList = (from i in db.Interventions
                                      where i.Client.ClientID == clientID
                                      select i).ToList();

            }

            return anInterventionList;
        }

        [NonAction]
        public List<Intervention> GetInterventionListByUserID(int userID)
        {
            List<Intervention> anInterventionList = new List<Intervention>();
            using (var db = new DBContext())
            {
                anInterventionList = (from i in db.Interventions
                                      where i.User.UserID == userID && (i.InterventionState == InterventionState.Approved || i.InterventionState == InterventionState.Completed)
                                      select i).ToList();

            }

            return anInterventionList;
        }

        [NonAction]
        public List<Intervention> GetInterventionList()
        {
            List<Intervention> anInterventionList = new List<Intervention>();
            using (var db = new DBContext())
            {
                anInterventionList = (from i in db.Interventions
                                      where i.InterventionState == InterventionState.Approved || i.InterventionState == InterventionState.Completed
                                      select i).ToList();

            }

            return anInterventionList;
        }

        [NonAction]
        public Intervention GetInterventionByInterventionID(int interventionID)
        {
            Intervention anIntervention = new Intervention();
            using (var db = new DBContext())
            {
                anIntervention = (from i in db.Interventions
                                  where i.InterventionID == interventionID
                                  select i).FirstOrDefault<Intervention>();

            }

            return anIntervention;
        }

        [NonAction]
        public List<Intervention> GetInterventionListByApprovalUserID(int userID)
        {
            List<Intervention> anInterventionList = new List<Intervention>();
            using (var db = new DBContext())
            {
                anInterventionList = (from i in db.Interventions
                                      where i.ApprovalUser.UserID == userID && i.InterventionState == InterventionState.Approved 
                                      select i).ToList();

            }

            return anInterventionList;
        }
    }
}
