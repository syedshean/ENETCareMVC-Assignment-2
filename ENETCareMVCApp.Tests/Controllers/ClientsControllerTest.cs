using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ENETCareMVCApp.Controllers;
using ENETCareMVCApp.Models;
using ENETCareMVCApp.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;

namespace ENETCareMVCApp.Tests.Controllers
{
    [TestClass]
    public class ClientsControllerTest
    {
        Client formData = new Client
        {
            ClientID = 100,
            ClientName = "Tom",
            Address = "Whatever",
            DistrictID = 1,
        };

        [TestMethod]
        public void CreateClient_PostOk()
        {
            Mock<IClientRepository> service = new Mock<IClientRepository>();
            service.Setup(x => x.AddClients(It.IsAny<Client>())).Returns<Client>(e => { e.ClientID = 100; return e; });
            var controller = new ClientsController(service.Object);
            
            RedirectToRouteResult result = (RedirectToRouteResult)controller.Create(formData);
            Assert.AreEqual(100, result.RouteValues["id"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Get_Correct_ClientList_By_District()
        {
            Mock<IClientRepository> service = new Mock<IClientRepository>();
            service.Setup(x => x.GetClientListByDistrict(It.IsAny<int>())).Returns<List<Client>>(e => { e[0].DistrictID = 1; return e; });
            var controller = new ClientsController(service.Object);

            List<Client> result = controller.GetClientListByDistrict(1);
            Assert.AreEqual(1, result[0].DistrictID);
        }
    }
}
