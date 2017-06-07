using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class InterventionTypeModelTest
    {
        [TestMethod]
        public void Given_InterventionTypeName_Is_Null_ExpectOneValidationError()
        {
            var model = new InterventionType()
            {
                InterventionTypeName = "",
                EstimatedCost = 2000,
                EstimatedLabour = 24,
                
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The InterventionTypeName field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_InterventionTypeName_Is_Valid_ExpectNoValidationError()
        {
            var model = new InterventionType()
            {
                InterventionTypeName = "Mosquito Net",
                EstimatedCost = 2000,
                EstimatedLabour = 24,
            

            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
           
        }

      
    }
}
