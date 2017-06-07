using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class InterventionModelTest
    {
        [TestMethod]
        public void Validate_Intervention_Model_Given_LabourHourRequired_Is_NegativeValue_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = -24,
                InterventionDate = "21/06/2017",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Labour Hour for Intervention", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_LabourHourRequired_Is_Unreasonable_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 1000,
                InterventionDate = "21/06/2017",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Labour Hour for Intervention", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_CostRequired_Is_NegativeValue_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = -2500,
                LabourRequired = 24,
                InterventionDate = "21/06/2017",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Cost for Intervention", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_CostRequired_Is_Unreasonable_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500000,
                LabourRequired = 24,
                InterventionDate = "21/06/2017",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Cost for Intervention", ErrorMeassageList[0].ErrorMessage);
        }

        //[TestMethod]
        //public void Validate_Model_Given_CostRequired_Is_Not_Valid_ExpectOneValidationError()
        //{
        //    var model = new Intervention()
        //    {
        //        CostRequired = ,
        //    };

        //    var results = TestModelHelper.Validate(model);

        //    Assert.AreEqual(3, results.Count);
        //    Assert.AreEqual("You entered an unreasonable Cost for Intervention", results[1].ErrorMessage);
        //}

        [TestMethod]
        public void Validate_Intervention_Model_Given_InterventionDate_Is_Null_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 24,
                InterventionDate = "",
            };
            

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.IsTrue(ErrorMeassageList[0].ErrorMessage != null);
            Assert.AreEqual("The Intervention Date field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_InterventionDate_Is_Valid_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 24,
                InterventionDate = "2017/06/6",
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);            
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_Note_Is_Null_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 24,
                InterventionDate = "2017/06/6",
                Notes = "",
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_RemainingLife_Is_Null_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 24,
                InterventionDate = "2017/06/6",
                Notes = "",
                RemainingLife = null,
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Validate_Intervention_Model_Given_LastEditDate_Is_Null_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = 2500,
                LabourRequired = 24,
                InterventionDate = "2017/06/6",
                RemainingLife = null,
                LastEditDate = "",
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }




    }
}
