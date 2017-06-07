using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;


namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class ClientModelTest
    {
        [TestMethod]
        public void Validate_Client_Model_Given_ClientName_Is_Null_ExpectOneValidationError()
        {
            var model = new Client()
            {
                ClientName = "",
                Address = "Rhodes",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The ClientName field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Validate_Client_Model_Given_Client_Address_Is_Invalid_ExpectOneValidationError()
        {
            var model = new Client()
            {
                ClientName = "Tomy",
                Address = 10000.ToString(),
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("Invalid  Address. Address only contains letters and space. Address has to be between 1 to 50 letters.", ErrorMeassageList[0].ErrorMessage);
        }
    }
}
