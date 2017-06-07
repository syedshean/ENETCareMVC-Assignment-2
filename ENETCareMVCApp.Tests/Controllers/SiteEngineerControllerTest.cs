using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Controllers;

namespace ENETCareMVCApp.Tests.Controllers
{
    [TestClass]
    public class SiteEngineerControllerTest
    {
        [TestMethod]
        public void TestControllerView()
        {
            var controller = new SiteEngineerController();
            var result = controller.Index("Hi") as ViewResult;
            Assert.AreEqual("Index", result.ViewName);

        }
    }
}
