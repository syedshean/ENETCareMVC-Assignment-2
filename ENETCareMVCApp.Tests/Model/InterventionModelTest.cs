using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class InterventionModelTest
    {
        [TestMethod]
        public void Validate_Model_Given_LabourHourRequired_Is_NegativeValuee_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = -1,
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(3, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Labour Hour for Intervention", ErrorMeassageList[0].ErrorMessage);
        }
        [TestMethod]
        public void Validate_Model_Given_CostRequired_Is_NegativeValue_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = -100,
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(3, ErrorMeassageList.Count);
            Assert.AreEqual("You entered an unreasonable Cost for Intervention", ErrorMeassageList[1].ErrorMessage);
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
        public void Validate_Model_Given_InterventionDate_Is_Null_ExpectOneValidationError()
        {
            var model = new Intervention();
            

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(3, ErrorMeassageList.Count);
            Assert.IsTrue(ErrorMeassageList[2].ErrorMessage != null);
            Assert.AreEqual("The InterventionDate field is required.", ErrorMeassageList[2].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Model_Given_InterventionDate_Is_Valid_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                InterventionDate = "2017/06/6",
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(2, ErrorMeassageList.Count);            
        }

        [TestMethod]
        public void Validate_Model_Given_InterventionDate_Is_InValid_ExpectNoValidationError()
        {
            var model = new Intervention()
            {
                InterventionDate = "aaaa",
            };


            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(3, ErrorMeassageList.Count);
        }




    }
}
