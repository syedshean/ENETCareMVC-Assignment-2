using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;
namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class UserModelTest
    {
        [TestMethod]       
        public void Validate_User_Model_Given_UserName_Is_Null_ExpectOneValidationError()
        {
            var model = new User();            

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(4, ErrorMeassageList.Count);
            Assert.AreEqual("The User Name field is required.", ErrorMeassageList[0].ErrorMessage);
        }


        [TestMethod]
        public void Validate_User_Model_Given_LoginName_Is_Null_ExpectOneValidationError()
        {
            var model = new User();

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(4, ErrorMeassageList.Count);
            Assert.AreEqual("The LoginName field is required.", ErrorMeassageList[1].ErrorMessage);
        }

        [TestMethod]
        public void Validate_User_Model_Given_Email_Is_Null_ExpectOneValidationError()
        {
            var model = new User();

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(4, ErrorMeassageList.Count);
            Assert.AreEqual("The Email field is required.", ErrorMeassageList[2].ErrorMessage);
        }

        [TestMethod]
        public void Validate_User_Model_Given_Email_Is_Not_Valid_ExpectOneValidationError()
        {
            var model = new User()
            {
                Email = "sejan",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(4, ErrorMeassageList.Count);
            Assert.AreEqual("The Email field is not a valid e-mail address.", ErrorMeassageList[2].ErrorMessage);
        }

        [TestMethod]
        public void Validate_User_Model_Given_UserType_Is_Null_ExpectOneValidationError()
        {
            var model = new User();

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(4, ErrorMeassageList.Count);
            Assert.AreEqual("The Role field is required.", ErrorMeassageList[3].ErrorMessage);
        }
    }
}
