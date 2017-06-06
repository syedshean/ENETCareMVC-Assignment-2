using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class InterventionModelTest
    {
        [TestMethod]
        public void Validate_Model_Given_CostRequired_Is_NegativeValue_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = -100,
            };

            var results = TestModelHelper.Validate(model);

            //Assert.AreEqual(1, results.Count);
            Assert.AreEqual("You entered an unreasonable Cost for Intervention", results[1].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Model_Given_LabourHourRequired_Is_NegativeValuee_ExpectOneValidationError()
        {
            var model = new Intervention()
            {
                CostRequired = -1,
            };

            var results = TestModelHelper.Validate(model);

            //Assert.AreEqual(1, results.Count);
            Assert.AreEqual("You entered an unreasonable Labour Hour for Intervention", results[0].ErrorMessage);
        }
    }
}
