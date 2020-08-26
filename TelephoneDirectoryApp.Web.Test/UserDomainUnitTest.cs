using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using TelephoneDirectoryApp.Business;
using Moq;
using TelephoneDirectoryApp.Contracts;
using TelephoneDirectoryApp.Entity.ViewModels;
using System.Linq;
using TelephoneDirectoryApp.Entity;

namespace TelephoneDirectoryApp.Web.Test
{
    [TestClass]
    public class UserDomainUnitTest
    {
        [TestMethod]
        public void GetUserProfileId_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.GetUserProfile(It.IsAny<int>())).Returns(() => new UserProfile() { Id = 1, Username ="ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email ="ntm1406@gmailcom"});

            UserDomain userDomain = new UserDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = userDomain.GetUserProfile(1);
            Assert.IsNotNull(response);

            Assert.AreEqual(1, response.Id);
            Assert.AreEqual("ntm1406", response.Username);
            Assert.IsNotNull(response.Password);
            Assert.IsNull(response.CreateBy);
        }

        [TestMethod]
        public void GetUserProfileUsername_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.GetUserProfile(It.IsAny<string>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });

            UserDomain userDomain = new UserDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = userDomain.GetUserProfile("ntm1406");
            Assert.IsNotNull(response);

            Assert.AreEqual(1, response.Id);
            Assert.AreEqual("ntm1406", response.Username);
            Assert.IsNotNull(response.Password);
            Assert.IsNull(response.CreateBy);
        }

        [TestMethod]
        public void GetAllProfiles_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 10, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });

            UserDomain userDomain = new UserDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = userDomain.GetAllUsers(null);
            Assert.IsNotNull(response);

            Assert.AreEqual(1, response.PageIndex);
            Assert.AreEqual(10, response.TotalItems);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual("Minh", response.Items.FirstOrDefault().FirstName);
        }

        [TestMethod]
        public void ValidateUser_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);

            UserDomain userDomain = new UserDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = userDomain.ValidateUser(null);
            Assert.IsTrue(response); 
        }

        /*[TestMethod]
        public void CreateUserProfile_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.CreateUserProfile(It.IsAny<UserProfile>()));

            UserDomain userDomain = new UserDomain(mockRepository.Object, mockLoggerRepository.Object);

            userDomain.CreateUserProfile(null);
            Assert.Fail();
        }*/

    }
}
