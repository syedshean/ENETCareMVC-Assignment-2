using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;


namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class DistrictModelTest
    {
        [TestMethod]
        public void Given_District_Is_Null_ExpectOneValidationError()
        {
            var model = new District()
            {
                DistrictName = "",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The DistrictName field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_District_Is_Valid_ExpectNoValidationError()
        {
            var model = new District()
            {
                DistrictName = "Sydney",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
            
        }
    }
}
