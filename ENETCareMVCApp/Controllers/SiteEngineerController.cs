using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ENETCareMVCApp.Controllers.ManageController;

namespace ENETCareMVCApp.Controllers
{
    public class SiteEngineerController : Controller
    {
        [Authorize(Roles = "SiteEngineer")]
        public ActionResult Index(String message)
        {
            ViewBag.StatusMessage = message;
            return View("Index");
        }

    }
}