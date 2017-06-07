using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ENETCareMVCApp.Models;

namespace ENETCareMVCApp.Tests.Model
{
    [TestClass]
    public class UserModelTest
    {
        [TestMethod]       
        public void Given_UserName_Is_Null_ExpectOneValidationError()
        {
            var model = new User()
            {
                UserName = "",
                LoginName = "Shean",
                Email = "ssss@gmail.com",
                UserType = "SiteEngineer",
            };            

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The User Name field is required.", ErrorMeassageList[0].ErrorMessage);
        }


        [TestMethod]
        public void Given_LoginName_Is_Null_ExpectOneValidationError()
        {
            var model = new User()
            {
                UserName = "Shean",
                LoginName = "",
                Email = "ssss@gmail.com",
                UserType = "SiteEngineer",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The LoginName field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_Email_Is_Null_ExpectOneValidationError()
        {
            var model = new User()
            {
                UserName = "Syed",
                LoginName = "Shean",
                Email = "",
                UserType = "SiteEngineer",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The Email field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_Email_Is_Not_Valid_ExpectOneValidationError()
        {
            var model = new User()
            {
                UserName = "Syed",
                LoginName = "Shean",
                Email = "sejan",
                UserType = "SiteEngineer",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The Email field is not a valid e-mail address.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_UserType_Is_Null_ExpectOneValidationError()
        {
            var model = new User()
            {
                UserName = "Syed",
                LoginName = "Shean",
                Email = "sejan@gmail.com",
                UserType = "",
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(1, ErrorMeassageList.Count);
            Assert.AreEqual("The Role field is required.", ErrorMeassageList[0].ErrorMessage);
        }

        [TestMethod]
        public void Given_MaxHour_Is_null_ExpectNoValidationError()
        {
            var model = new User()
            {
                UserName = "Syed",
                LoginName = "Shean",
                Email = "ssss@gmail.com",
                UserType = "SiteEngineer",
                MaxHour = null,
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }

        [TestMethod]
        public void Given_MaxCost_Is_null_ExpectNoValidationError()
        {
            var model = new User()
            {
                UserName = "Syed",
                LoginName = "Shean",
                Email = "ssss@gmail.com",
                UserType = "SiteEngineer",
                MaxHour = null,
                MaxCost = null,
            };

            var ErrorMeassageList = TestModelHelper.Validate(model);

            Assert.AreEqual(0, ErrorMeassageList.Count);
        }
    }
}
