using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using TelephoneDirectoryApp.Contracts;
using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;
using TelephoneDirectoryApp.Web.UI.Controllers;

namespace TelephoneDirectoryApp.Web.Test
{
    [TestClass]
    public class ContactControllerUnitTest
    {
        [TestMethod]
        public void AllContactsViewSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<string>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });


            var mockContactDomain = new Mock<IContactDomain>();
            mockContactDomain.Setup(p => p.GetAllContacts(It.IsAny<ContactSearchViewModel>())).Returns(() => new AllContactsViewModel() { PageIndex = 1, TotalItems = 10, Items = new List<UserContact>() { new UserContact() { Id = 1, FirstName = "satya", LastName = "palakurla", Email = "satya@gmail.com" } } });
            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));


            ContactController contactController = new ContactController(mockContactDomain.Object, mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            contactController.ControllerContext = context;
            var result = contactController.AllContactsView(new ContactSearchViewModel { FirstNameSearch = null, LastNameSearch = null, PhoneSearch = null, PageIndex = 1, PageSize = 5 }, 0) as ViewResult;
            Assert.IsNotNull(result);

            var userContactList = result.ViewData.Model as List<UserContact>;
            Assert.IsNotNull(userContactList);


            Assert.AreEqual(1, userContactList.Count);

            Assert.AreEqual("satya@gmail.com", userContactList.FirstOrDefault().Email);

        }

        [TestMethod]
        public void EditContactSuccess()
        {
            var mockUserDomain = new Mock<IUserDomain>();

            mockUserDomain.Setup(p => p.GetAllUsers(It.IsAny<UserSearchViewModel>())).Returns(() => new AllProfilesViewModel() { PageIndex = 1, TotalItems = 5, Items = new List<UserProfile>() { new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" } } });
            mockUserDomain.Setup(p => p.ValidateUser(It.IsAny<LoginViewModel>())).Returns(() => true);
            mockUserDomain.Setup(p => p.GetUserProfile(It.IsAny<string>())).Returns(() => new UserProfile() { Id = 1, Username = "ntm1406", Password = "1234", FirstName = "Minh", LastName = "Nguyen", Email = "ntm1406@gmailcom" });


            var mockContactDomain = new Mock<IContactDomain>();
            mockContactDomain.Setup(p => p.GetAllContacts(It.IsAny<ContactSearchViewModel>())).Returns(() => new AllContactsViewModel() { PageIndex = 1, TotalItems = 10, Items = new List<UserContact>() { new UserContact() { Id = 1, FirstName = "satya", LastName = "palakurla", Email = "satya@gmail.com" } } });
            mockContactDomain.Setup(p => p.GetUserContact(It.IsAny<int>())).Returns(() => new UserContact() { Id = 1, FirstName = "satya", LastName = "palakurla", Email = "satya@gmail.com" } );
            var mockLoggerDomain = new Mock<ILogger>();

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "ntm1406"),
                new Claim(ClaimTypes.Role, "Admin"),
            }, CookieAuthenticationDefaults.AuthenticationScheme));


            ContactController contactController = new ContactController(mockContactDomain.Object, mockUserDomain.Object, mockLoggerDomain.Object);
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            contactController.ControllerContext = context;
            var result = contactController.EditContact(1) as ViewResult;
            Assert.IsNotNull(result);
        }

    }
}
