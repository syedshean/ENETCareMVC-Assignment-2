using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENETCareMVCApp.Controllers
{
    public class AccountantController : Controller
    {
        [Authorize(Roles = "Accountant")]
        public ActionResult Index(String message)
        {
            ViewBag.StatusMessage = message;
            return View();
        }
    }
}