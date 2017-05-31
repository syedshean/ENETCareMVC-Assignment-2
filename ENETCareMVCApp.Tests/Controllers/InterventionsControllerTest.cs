using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp;
using ENETCareMVCApp.Controllers;

namespace ENETCareMVCApp.Tests.Controllers
{
    [TestClass]
    public class InterventionsControllerTest
    {
        [TestMethod]
        public void TestPreviousInterventionListView()
        {
            var controller = new InterventionsController();
            var result = controller.PreviousInterventionList() as ViewResult;
            Assert.AreEqual("PreviousInterventionList", result.ViewName);
        }
    }
}
