using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ENETCareMVCApp.Models;
using System.Globalization;

namespace ENETCareMVCApp.Controllers
{
    public class InterventionsController : Controller
    {
        private DBContext db = new DBContext();


        // GET: Interventions
        public ActionResult Index()
        {
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType);
            return View("Index", interventions.ToList());
        }

        public ActionResult ProposedInterventionList()
        {
            string logedUser = User.Identity.Name;
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Include(i =>i.User).Where(i=>i.InterventionState==InterventionState.Proposed).Where(i=>i.Client.DistrictID==districtID);
            return View("ProposedInterventionList", interventions.ToList());
        }

        public ActionResult ManagerApprovedInterventionList()
        {
            string logedUser = User.Identity.Name;
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            int userID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Include(i => i.User).Where(i => i.InterventionState == InterventionState.Approved).Where(i=>i.ApproveUserID==userID);
            return View("ManagerApprovedInterventionList", interventions.ToList());
        }

        public ActionResult PreviousInterventionList()
        {
            string logedUser = User.Identity.Name;
            int userID = db.Users.Where(i=>i.LoginName==logedUser).FirstOrDefault().UserID;
            //int userID = Int32.Parse(GetUserIDByUserName(logedUser));
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Where(i=>i.UserID== userID);
            return View("PreviousInterventionList", interventions.ToList());
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
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID; 
            ViewBag.ClientID = new SelectList(db.Clients.Where(i=>i.DistrictID==districtID), "ClientID", "ClientName");
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName");
            return View("Create", aInterventionModel);
        }

        // POST: Interventions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,UserID,InterventionTypeID")] Intervention intervention)
        {
            intervention.RemainingLife = 100;
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

            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            ViewBag.ClientID = new SelectList(db.Clients.Where(i => i.DistrictID == districtID), "ClientID", "ClientName", intervention.ClientID);
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
        public ActionResult Edit([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,UserID,InterventionTypeID")] Intervention intervention)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(intervention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PreviousInterventionList");
            }
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        // GET: Interventions/Edit/5
        public ActionResult ChangeInterventionState(int? id)
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
            string logedUser = User.Identity.Name;
            //int userID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            intervention.ApproveUserID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View("ChangeInterventionState", intervention);
        }

        // POST: Interventions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeInterventionState([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,UserID,ApproveUserID,InterventionTypeID")] Intervention intervention)
        {
            string logedUser = User.Identity.Name;
            string userType = db.Users.Where(i => i.LoginName== logedUser).FirstOrDefault().UserType;
            if (ModelState.IsValid)
            {
                db.Entry(intervention).State = EntityState.Modified;
                db.SaveChanges();
                if (userType == "Manager")
                {
                    return RedirectToAction("ProposedInterventionList");
                }
                else
                {
                    return RedirectToAction("PreviousInterventionList");
                }
                
            }
            //string logedUser = User.Identity.Name;
            //int userID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            intervention.ApproveUserID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
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

        public ActionResult EditQMI(int? id)
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
            if ((intervention.InterventionState == InterventionState.Proposed) || (intervention.InterventionState == InterventionState.Approved))
            {
                ViewBag.Disabled = true;
            }
            else
                ViewBag.Disabled = false;
            string lastEditDate = intervention.LastEditDate;
            if (lastEditDate == null)
                intervention.LastEditDate = "Never updated before";
            ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQMI([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,InterventionTypeID, Notes, RemainingLife")] Intervention intervention) //InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,InterventionTypeID
        {
            if (ModelState.IsValid)
            {
                intervention.LastEditDate = DateTime.Now.ToString("yyyy-MM-dd");
                db.Interventions.Attach(intervention);
                //db.Entry(intervention).Property(i => i.InterventionState).IsModified = true;
                db.Entry(intervention).Property(i => i.Notes).IsModified = true;
                db.Entry(intervention).Property(i => i.RemainingLife).IsModified = true;
                db.Entry(intervention).Property(i => i.LastEditDate).IsModified = true;
                //db.Entry(intervention).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ClientListWithIntervention","Clients");
            }
            Intervention anIntervention = db.Interventions.Find(intervention.InterventionID);
            if ((intervention.InterventionState == InterventionState.Proposed) || (intervention.InterventionState == InterventionState.Approved))
            {
                ViewBag.Disabled = true;
            }
            else
                ViewBag.Disabled = false;

            string lastEditDate = anIntervention.LastEditDate;
            if (lastEditDate == null)
                anIntervention.LastEditDate = "Never updated before";
            return View(anIntervention);
        }

        public ActionResult TotalCostByEngineerReport()
        {
            List<SiteEngineerTotalCost> resultList = new List<SiteEngineerTotalCost>();
            List<User> anUserList = db.Users.Where(u => u.UserType == "SiteEngineer").OrderBy(u => u.UserName).ToList();
            foreach (var user in anUserList)
            {
                SiteEngineerTotalCost result = db.Interventions
                           .Where(i => (i.InterventionState == InterventionState.Completed) && (i.UserID == user.UserID))
                           .GroupBy(i => i.UserID)
                           .Select(setc => new SiteEngineerTotalCost
                           {
                               UserName = user.UserName,
                               TotalCost = setc.Sum(i => i.CostRequired).ToString(),
                               TotalLabour = setc.Sum(i => i.LabourRequired).ToString(),
                               DistrictName = setc.FirstOrDefault().User.District.DistrictName
                           }).FirstOrDefault();
                if (result == null)
                {
                    result = new SiteEngineerTotalCost();
                    result.UserName = user.UserName;
                    result.TotalCost = "0";
                    result.TotalLabour = "0";
                    result.DistrictName = user.District.DistrictName;
                }
                resultList.Add(result);
            }
            return View(resultList);
        }

        public ActionResult AverageCostByEngineerReport()
        {
            List<SiteEngineerTotalCost> resultList = new List<SiteEngineerTotalCost>();
            List<User> anUserList = db.Users.Where(u => u.UserType == "SiteEngineer").OrderBy(u => u.UserName).ToList();
            foreach (var user in anUserList)
            {
                SiteEngineerTotalCost result = db.Interventions
                           .Where(i => (i.InterventionState == InterventionState.Completed) && (i.UserID == user.UserID))
                           .GroupBy(i => i.UserID)
                           .Select(setc => new SiteEngineerTotalCost
                           {
                               UserName = user.UserName,
                               TotalCost = setc.Average(i => i.CostRequired).ToString(),
                               TotalLabour = setc.Average(i => i.LabourRequired).ToString(),
                               DistrictName = setc.FirstOrDefault().User.District.DistrictName
                           }).FirstOrDefault();
                if (result == null)
                {
                    result = new SiteEngineerTotalCost();
                    result.UserName = user.UserName;
                    result.TotalCost = "0";
                    result.TotalLabour = "0";
                    result.DistrictName = user.District.DistrictName;
                }
                resultList.Add(result);
            }
            return View(resultList);
        }

        public ActionResult CostByDistrictReport()
        {
            //var costByDistrictList = db.Interventions.Include(i=>i.)

            return View();
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

        [NonAction]
        public string GetDistrictIDByUserName(string userName)
        {
            string districtID;
            using (var db = new DBContext())
            {
                districtID = (from u in db.Users
                          where u.LoginName == userName
                          select u.DistrictID).FirstOrDefault().ToString();

            }
            return districtID;
        }
    }
}
