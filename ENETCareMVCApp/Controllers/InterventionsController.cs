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
using System.Net.Mail;
using ENETCareMVCApp.Repositories;

namespace ENETCareMVCApp.Controllers
{
    public class InterventionsController : Controller
    {
        private DBContext db = new DBContext();
        IInterventionRepository repository;

        public InterventionsController()
        {
            db = new DBContext();
            repository = new InterventionRepository();
        }

        public InterventionsController(IInterventionRepository repository)
        {
            this.repository = repository;
        }

        // GET: Interventions
        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Index()//Needs to edit check asngmt document Bus rules
        {
            string logedUser = User.Identity.Name;
            int userID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Where(i => i.UserID == userID).Where(i => i.InterventionState != InterventionState.Cancelled);
            return View("Index", interventions.ToList());
        }

        [Authorize(Roles = "Manager")]
        public ActionResult ProposedInterventionList()
        {
            string logedUser = User.Identity.Name;
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Include(i =>i.User).Where(i=>i.InterventionState==InterventionState.Proposed).Where(i=>i.Client.DistrictID==districtID);
            return View("ProposedInterventionList", interventions.ToList());
        }

        [Authorize(Roles = "Manager")]
        public ActionResult ManagerApprovedInterventionList()
        {
            string logedUser = User.Identity.Name;
            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            int userID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserID;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Include(i => i.User).Where(i => i.InterventionState == InterventionState.Approved).Where(i=>i.ApproveUserID==userID);
            return View("ManagerApprovedInterventionList", interventions.ToList());
        }

        [Authorize(Roles = "SiteEngineer")]
        public ActionResult PreviousInterventionList()//Needs to edit check asngmt document Bus rules
        {
            string logedUser = User.Identity.Name;
            int userID = db.Users.Where(i=>i.LoginName==logedUser).FirstOrDefault().UserID;
            var MaxSiteEngineerCost = db.Users.Where(u => u.LoginName == logedUser).FirstOrDefault().MaxCost;
            var MaxSiteEngineerHour = db.Users.Where(u => u.LoginName == logedUser).FirstOrDefault().MaxHour;
            var interventions = db.Interventions.Include(i => i.Client).Include(i => i.InterventionType).Where(i=>i.UserID== userID).Where(i=>i.InterventionState!= InterventionState.Cancelled).Where(i => i.CostRequired <= MaxSiteEngineerCost).Where(i => i.LabourRequired <= MaxSiteEngineerHour);
            return View("PreviousInterventionList", interventions.ToList());
        }

        // GET: Interventions/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Intervention intervention = db.Interventions.Find(id);
        //    if (intervention == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(intervention);
        //}

        // GET: Interventions/Create
        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Create()
        {
            ViewBag.SiteEngineerOutofLimit = false;
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
            string logedUser = User.Identity.Name;
            var MaxSiteEngineerCost = db.Users.Where(u => u.LoginName == logedUser).FirstOrDefault().MaxCost;
            var MaxSiteEngineerHour = db.Users.Where(u => u.LoginName == logedUser).FirstOrDefault().MaxHour;
            ViewBag.SiteEngineerOutofLimit = false;

            if (ModelState.IsValid)
            {
                if (((intervention.CostRequired > MaxSiteEngineerCost) || (intervention.LabourRequired > MaxSiteEngineerHour)) && (intervention.InterventionState != InterventionState.Proposed))
                {
                    ViewBag.SiteEngineerOutofLimit = true;
                    ViewBag.SiteEngineerOutofLimitMessage = "Sorry this intervention is out of your limit. You must propose this intervention";

                }
                else
                {
                    db.Interventions.Add(intervention);
                    db.SaveChanges();
                    return RedirectToAction("PreviousInterventionList");
                }
            }
            
            User currentUser = (db.Users.Where(u => u.LoginName == logedUser)).First();
            intervention.UserID = currentUser.UserID;
            intervention.User = currentUser;

            int districtID = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().DistrictID;
            ViewBag.ClientID = new SelectList(db.Clients.Where(i => i.DistrictID == districtID), "ClientID", "ClientName", intervention.ClientID);
            ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
            return View(intervention);
        }

        // GET: Interventions/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Intervention intervention = db.Interventions.Find(id);
        //    if (intervention == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
        //    ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
        //    return View(intervention);
        //}

        //// POST: Interventions/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "InterventionID,LabourRequired,CostRequired,InterventionDate,InterventionState,ClientID,UserID,InterventionTypeID")] Intervention intervention)
        //{
        //    if (ModelState.IsValid)
        //    {                
        //        db.Entry(intervention).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("PreviousInterventionList");
        //    }
        //    ViewBag.ClientID = new SelectList(db.Clients, "ClientID", "ClientName", intervention.ClientID);
        //    ViewBag.InterventionTypeID = new SelectList(db.InterventionTypes, "InterventionTypeID", "InterventionTypeName", intervention.InterventionTypeID);
        //    return View(intervention);
        //}

        [Authorize(Roles = "SiteEngineer, Manager")]
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
            string userType = db.Users.Where(i => i.LoginName == logedUser).FirstOrDefault().UserType;
            var changeInterventionState = InterventionState.Cancelled;
            if (userType == "SiteEngineer")
            {
                ViewBag.SiteEngineerInterventionState = true;
                if(intervention.InterventionState == InterventionState.Proposed)
                {
                    changeInterventionState = InterventionState.Proposed;
                }
                else if (intervention.InterventionState == InterventionState.Approved)
                {
                    changeInterventionState = InterventionState.Approved;
                }
            }
            else
            {
                ViewBag.SiteEngineerInterventionState = false;
            }
            var SiteEngineerSelectList = Enum.GetValues(typeof(InterventionState))
                       .Cast<InterventionState>()
                       .Where(e => e > changeInterventionState)
                       .Select(e => new SelectListItem
                       {
                           Value = ((int)e).ToString(),
                           Text = e.ToString()
                       });
            ViewBag.SiteEngineerInterventionStateList = SiteEngineerSelectList;

            if (userType == "Manager")
            {
                ViewBag.ManagerApproveState = true;                
            }
            else
            {
                ViewBag.ManagerApproveState = false;
            }

            var managerSelectList = Enum.GetValues(typeof(InterventionState))
                        .Cast<InterventionState>()
                        .Where(e => e != InterventionState.Proposed).Where(e => e != InterventionState.Completed)
                        .Select(e => new SelectListItem
                        {
                            Value = ((int)e).ToString(),
                            Text = e.ToString()
                        });
            ViewBag.ManagerInterventionStateList = managerSelectList;

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

                // Email sent 
                if (userType == "Manager")
                {
                    string emailAddr = db.Users.Where(i => i.UserID == intervention.UserID).FirstOrDefault().Email;

                    try
                    {
                        
                        MailMessage mailMessage = new MailMessage();

                        mailMessage.To.Add(emailAddr);
                        mailMessage.From = new MailAddress("ENETCare.UltimoCoder@gmail.com");
                        mailMessage.Subject = "Intervention state updated";
                        mailMessage.Body = "Hello your Intervention state was just updated. The status of your intervention is now "+ intervention.InterventionState.ToString(); //\"" + intervention.InterventionType.InterventionTypeName +"\" for client "+ intervention.Client.ClientName +"

                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                        smtpClient.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("ENETCare.UltimoCoder@gmail.com", "Aa!12345678");
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = NetworkCred;
                        smtpClient.Port = 587;
                        smtpClient.Send(mailMessage);

                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }

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

        //// GET: Interventions/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Intervention intervention = db.Interventions.Find(id);
        //    if (intervention == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(intervention);
        //}

        //// POST: Interventions/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Intervention intervention = db.Interventions.Find(id);
        //    db.Interventions.Remove(intervention);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        [Authorize(Roles = "SiteEngineer")]
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
                Intervention returnIntervention = repository.EditIntervention(intervention);
                return RedirectToAction("ClientListWithIntervention","Clients", new { id = returnIntervention.InterventionID});
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

        [Authorize(Roles = "Accountant")]
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

        [Authorize(Roles = "Accountant")]
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

        [Authorize(Roles = "Accountant")]
        public ActionResult CostByDistrictReport()
        {
            List<CostByDistrict> resultList = new List<CostByDistrict>();
            List<District> aDistrictList = db.Districts.ToList();
            foreach (var district in aDistrictList)
            {
                CostByDistrict result = db.Interventions
                           .Where(i => i.InterventionState == InterventionState.Completed).Where(i => i.User.DistrictID == district.DistrictID)
                           .GroupBy(i => i.User.DistrictID)
                           .Select(set => new CostByDistrict
                           {
                               DistrictName = district.DistrictName,
                               TotalCost = set.Sum(i => i.CostRequired).ToString(),
                               TotalLabour = set.Sum(i => i.LabourRequired).ToString(),
                               
                           }).FirstOrDefault();
                if (result == null)
                {
                    result = new CostByDistrict();
                    result.DistrictName = district.DistrictName;
                    result.TotalCost = "0";
                    result.TotalLabour = "0";
                    
                }
                resultList.Add(result);
            }
            return View(resultList);
        }

        [Authorize(Roles = "Accountant")]
        public ActionResult DistrictListForMonthlyCost()
        {
            
            return View(db.Districts.ToList());
        }

        [Authorize(Roles = "Accountant")]
        public ActionResult MonthlyCostsForDistrict(int? id)
        {
            List<CostByDistrict> resultList = new List<CostByDistrict>();
            int[] monthList = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            
            foreach (var month in monthList)
            {
                CostByDistrict result = db.Interventions.ToList()
                           .Where(i => i.InterventionState == InterventionState.Completed).Where(i => i.User.DistrictID == id).Where(i => DateTime.Parse(i.InterventionDate, CultureInfo.CurrentCulture).Month == month)
                           .GroupBy(i => i.InterventionDate)
                           .Select(set => new CostByDistrict
                           {                               
                               TotalCost = set.Sum(i => i.CostRequired).ToString(),
                               TotalLabour = set.Sum(i => i.LabourRequired).ToString(),
                               Date = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName (month),
                               

                           }).FirstOrDefault();
                if (result == null)
                {
                    result = new CostByDistrict();                    
                    result.Date = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                    result.TotalCost = "0";
                    result.TotalLabour = "0";

                }
                resultList.Add(result);
            }
            return View(resultList);
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
