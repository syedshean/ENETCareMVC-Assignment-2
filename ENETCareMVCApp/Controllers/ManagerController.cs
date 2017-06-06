using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCareMVCApp.Controllers
{
    public class ManagerController : Controller
    {
        [Authorize(Roles = "Manager")]
        public ActionResult Index(String message)
        {
            ViewBag.StatusMessage = message;
            return View();
        }
    }
}