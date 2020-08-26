using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TelephoneDirectoryApp.Contracts;
using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;
using TelephoneDirectoryApp.Web.UI.Controllers;

namespace TelephoneDirectoryApp.Web.Test
{
    [TestClass]
    public class AccountControllerUnitTest
    {
        [TestMethod]
        public void ProfileViewSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<int>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });


            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, "Admin"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));



            AccountController accountController = new AccountController(mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            accountController.ControllerContext = context;
            var result = accountController.ProfileView(1) as ViewResult;
            Assert.IsNotNull(result);

            var userProfile = result.ViewData.Model as UserProfile;
            Assert.IsNotNull(userProfile);
            Assert.AreEqual("Minh", userProfile.FirstName);
        }

        [TestMethod]
        public void AllProfilesViewSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);


            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));


            AccountController accountController = new AccountController(mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            accountController.ControllerContext = context;
            var result = accountController.AllProfilesView(new UserSearchViewModel { UsernameSearch = null, FirstNameSearch = null, LastNameSearch = null, EmailSearch = null, PageIndex = 1, PageSize = 5 }, 0) as ViewResult;
            Assert.IsNotNull(result);

            var userProfileList = result.ViewData.Model as List<UserProfile>;
            Assert.IsNotNull(userProfileList);
            Assert.AreEqual(1, userProfileList.Count);

        }

        [TestMethod]
        public void EditProfileSucces()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<int>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });

            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));



            AccountController accountController = new AccountController(mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };  

            accountController.ControllerContext = context;
            var result = accountController.EditProfile() as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ResetPasswordSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<int>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });

            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));



            AccountController accountController = new AccountController(mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            accountController.ControllerContext = context;
            var result = accountController.ResetPassword() as ViewResult;
            Assert.IsNotNull(result);
        }
        [TestMethod]
        public void RemoveProfileSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<int>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });

            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));



            AccountController accountController = new AccountController(mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            accountController.ControllerContext = context;
            var result = accountController.RemoveProfile(1) as ViewResult;
            Assert.IsNotNull(result);
        }
    }
}
