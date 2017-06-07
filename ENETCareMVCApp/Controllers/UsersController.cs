using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Controllers
{
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();

        [Authorize(Roles = "Accountant")]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.District).Where(i=>i.UserType!= "Accountant");
            return View(users.ToList());
        }
        
        [Authorize(Roles = "Accountant")]
        public ActionResult ChangeDistrict(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", user.DistrictID);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeDistrict([Bind(Include = "UserID,UserName,LoginName,Email,UserType,MaxHour,MaxCost,DistrictID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "DistrictID", "DistrictName", user.DistrictID);
            return View(user);
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
