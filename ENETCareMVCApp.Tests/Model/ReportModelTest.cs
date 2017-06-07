using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class ReportModelTest
    {
        [TestMethod]
        public void Validate_Report_For_Model_SiteEngineerTotalCost_Given_Null_ExpectNoValidationError()
        {
            var model = new SiteEngineerTotalCost();

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Validate_Report_For_Model_SiteEngineerTotalCost_Given_Valid_ExpectNoValidationError()
        {
            var model = new SiteEngineerTotalCost()
            {
                UserName = "Tobby",
                TotalCost = "56",
                TotalLabour = "35",
                DistrictName = "Rhodes"
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Validate_Report_For_Model_ClientWithInterventionModel_Given_Valid_ExpectNoValidationError()
        {
            var model = new ClientWithInterventionModel()
            {
                
                DistrictName = "Rhodes",
                InterventionState = "Completed",
                InterventionTypeName = "Mosquito Net",

            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Validate_Report_For_CostByDistrict_Given_Valid_ExpectNoValidationError()
        {
            var model = new CostByDistrict()
            {
                Date = "7/06/2017",
                TotalCost = "56",
                TotalLabour = "35",
                DistrictName = "Rhodes"
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }



    }


}
