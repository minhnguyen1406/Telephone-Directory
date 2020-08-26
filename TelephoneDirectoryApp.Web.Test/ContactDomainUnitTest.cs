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
    public class ContactDomainUnitTest
    {

        [TestMethod]
        public void GetUserContactId_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.GetUserContact(It.IsAny<int>())).Returns(() => new UserContact() { 
                Id = 1, FirstName = "Minh", LastName = "Nguyen", 
                Email = "ntm1406@gmailcom", Gender = "Male", Phone = "8177736020",
                City = "Fort Worth", State = "Texas", Zip = "76110", UserId = 1,
                UserProfile = new UserProfile {Id = 1, Username ="jsmith", Password = "1234", FirstName = "John", LastName = "Smith", Email ="john@mail.com"}
            });

            ContactDomain contactDomain = new ContactDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = contactDomain.GetUserContact(1);
            Assert.IsNotNull(response);

            Assert.AreEqual(1, response.Id);
            Assert.AreEqual("Minh", response.FirstName);
            Assert.AreEqual("8177736020", response.Phone);
            Assert.AreEqual("jsmith", response.UserProfile.Username);
            Assert.IsNotNull(response.UserProfile.Password);
            Assert.IsNull(response.UserProfile.CreateBy);
        }

        [TestMethod]
        public void GetAllContacts_UnitTestSuccess()
        {
            var mockRepository = new Mock<IRepository>();
            var mockLoggerRepository = new Mock<ILogger>();
            mockRepository.Setup(p => p.GetAllContacts(It.IsAny<ContactSearchViewModel>())).Returns(() => new AllContactsViewModel() { PageIndex = 1, TotalItems = 10, Items = new List<UserContact>() {
                new UserContact() { Id = 1, FirstName = "Minh", LastName = "Nguyen",
                Email = "ntm1406@gmailcom", Gender = "Male", Phone = "8177736020",
                City = "Fort Worth", State = "Texas", Zip = "76110", UserId = 1,
                UserProfile = new UserProfile {Id = 1, Username ="jsmith", Password = "1234", FirstName = "John", LastName = "Smith", Email ="john@mail.com"} } } });

            ContactDomain contactDomain = new ContactDomain(mockRepository.Object, mockLoggerRepository.Object);

            var response = contactDomain.GetAllContacts(null);
            Assert.IsNotNull(response);

            Assert.AreEqual(1, response.PageIndex);
            Assert.AreEqual(10, response.TotalItems);
            Assert.IsNotNull(response.Items);
            Assert.AreEqual("Minh", response.Items.FirstOrDefault().FirstName);
        }

    }
}
