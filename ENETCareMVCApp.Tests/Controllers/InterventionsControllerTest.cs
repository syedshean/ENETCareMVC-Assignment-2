using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp;
using ENETCareMVCApp.Controllers;
using ENETCareMVCApp.Repositories;
using Moq;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Controllers
{
    [TestClass]
    public class InterventionsControllerTest
    {
        Intervention formData = new Intervention
        {
            ClientID = 100,
            InterventionID = 1,
            InterventionState = InterventionState.Approved,
            InterventionTypeID = 1,
            InterventionDate = "2017-04-12",
            LabourRequired = 20,
            CostRequired = 2000,
            UserID = 1

        };
        [TestMethod]
        public void TestPreviousInterventionListView()
        {
            var controller = new InterventionsController();
            var result = controller.PreviousInterventionList() as ViewResult;
            Assert.AreEqual("PreviousInterventionList", result.ViewName);
        }

        [TestMethod]
        public void EditQMI_PostOk()
        {
            Mock<IInterventionRepository> service = new Mock<IInterventionRepository>();
            service.Setup(x => x.EditIntervention(It.IsAny<Intervention>())).Returns<Intervention>(e => { e.InterventionID = 1; return e; });
            var controller = new InterventionsController(service.Object);

            RedirectToRouteResult result = (RedirectToRouteResult)controller.EditQMI(formData);
            Assert.AreEqual(1, result.RouteValues["id"]);
            Assert.AreEqual("ClientListWithIntervention", result.RouteValues["action"]);
        }
    }
}
