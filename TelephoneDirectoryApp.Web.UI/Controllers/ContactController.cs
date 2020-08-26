using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using TelephoneDirectoryApp.ConsoleLogger;
using TelephoneDirectoryApp.Contracts;
using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;
using System.Linq;

namespace TelephoneDirectoryApp.Web.UI.Controllers
{
    public class ContactController : Controller
    {
        private IContactDomain contactDomain;
        private IUserDomain userDomain;
        private ILogger logger;

        public string LoggedUser
        {
            get
            {
                return User.Identity.Name;
            }
        }

        public List<SelectListItem> stateList
        {
            get
            {
                return new List<SelectListItem> {
                    new SelectListItem { Value = "", Text = "" },
                    new SelectListItem { Value = "AL", Text = "Alabama" },
                    new SelectListItem { Value = "AK", Text = "Alaska" },
                    new SelectListItem { Value = "AZ", Text = "Arizona" },
                    new SelectListItem { Value = "AR", Text = "Arkansas" },
                    new SelectListItem { Value = "CA", Text = "California" },
                    new SelectListItem { Value = "CO", Text = "Colorado" },
                    new SelectListItem { Value = "CT", Text = "Connecticut" },
                    new SelectListItem { Value = "DE", Text = "Delaware" },
                    new SelectListItem { Value = "FL", Text = "Florida" },
                    new SelectListItem { Value = "GA", Text = "Georgia" },
                    new SelectListItem { Value = "HI", Text = "Hawaii" },
                    new SelectListItem { Value = "ID", Text = "Idaho" },
                    new SelectListItem { Value = "IL", Text = "Illinois" },
                    new SelectListItem { Value = "IN", Text = "Indiana" },
                    new SelectListItem { Value = "IA", Text = "Iowa" },
                    new SelectListItem { Value = "KS", Text = "Kansas" },
                    new SelectListItem { Value = "KY", Text = "Kentucky" },
                    new SelectListItem { Value = "LA", Text = "Louisiana" },
                    new SelectListItem { Value = "ME", Text = "Maine" },
                    new SelectListItem { Value = "MD", Text = "Maryland" },
                    new SelectListItem { Value = "MA", Text = "Massachusetts" },
                    new SelectListItem { Value = "MI", Text = "Michigan" },
                    new SelectListItem { Value = "MN", Text = "Minnesota" },
                    new SelectListItem { Value = "MS", Text = "Mississippi" },
                    new SelectListItem { Value = "MO", Text = "Missouri" },
                    new SelectListItem { Value = "MT", Text = "Montana" },
                    new SelectListItem { Value = "NC", Text = "North Carolina" },
                    new SelectListItem { Value = "ND", Text = "North Dakota" },
                    new SelectListItem { Value = "NE", Text = "Nebraska" },
                    new SelectListItem { Value = "NV", Text = "Nevada" },
                    new SelectListItem { Value = "NH", Text = "New Hampshire" },
                    new SelectListItem { Value = "NJ", Text = "New Jersey" },
                    new SelectListItem { Value = "NM", Text = "New Mexico" },
                    new SelectListItem { Value = "NY", Text = "New York" },
                    new SelectListItem { Value = "OH", Text = "Ohio" },
                    new SelectListItem { Value = "OK", Text = "Oklahoma" },
                    new SelectListItem { Value = "OR", Text = "Oregon" },
                    new SelectListItem { Value = "PA", Text = "Pennsylvania" },
                    new SelectListItem { Value = "RI", Text = "Rhode Island" },
                    new SelectListItem { Value = "SC", Text = "South Carolina" },
                    new SelectListItem { Value = "SD", Text = "South Dakota" },
                    new SelectListItem { Value = "TN", Text = "Tennessee" },
                    new SelectListItem { Value = "TX", Text = "Texas" },
                    new SelectListItem { Value = "UT", Text = "Utah" },
                    new SelectListItem { Value = "VT", Text = "Vermont" },
                    new SelectListItem { Value = "VA", Text = "Virginia" },
                    new SelectListItem { Value = "WA", Text = "Washington" },
                    new SelectListItem { Value = "WV", Text = "West Virginia" },
                    new SelectListItem { Value = "WI", Text = "Wisconsin" },
                    new SelectListItem { Value = "WY", Text = "Wyoming" }
                };
            }
        }

        public ContactController(IContactDomain contactDomain, IUserDomain userDomain, ILogger logger)
        {
            this.contactDomain = contactDomain;
            this.userDomain = userDomain;
            this.logger = logger;
        }

        public IActionResult AllContactsView(ContactSearchViewModel contactSearchViewModel, int pageIndex = 0)
        {
            ViewData["ItemsPerPage"] = new List<SelectListItem>
                    {
                        new SelectListItem("Items Per Page", "0"),
                        new SelectListItem("5", "5"),
                        new SelectListItem("10", "10"),
                        new SelectListItem("15", "15"),
                        new SelectListItem("20", "20")
            };
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                var contactSearchDisplayViewModel = new ContactSearchViewModel()
                {
                    FirstNameSearch = contactSearchViewModel.FirstNameSearch,
                    LastNameSearch = contactSearchViewModel.LastNameSearch,
                    PhoneSearch = contactSearchViewModel.PhoneSearch,
                    UserId = this.userDomain.GetUserProfile(LoggedUser).Id,
                    PageIndex = pageIndex > 0 ? pageIndex : 1,
                    PageSize = Convert.ToInt32(contactSearchViewModel.PageSize)
                };

                ViewData["pageIndex"] = contactSearchDisplayViewModel.PageIndex;
                ViewData["pageSize"] = contactSearchDisplayViewModel.PageSize;

                var allContactsViewModel = this.contactDomain.GetAllContacts(contactSearchDisplayViewModel);
                ViewData["totalItems"] = allContactsViewModel.TotalItems;
                return View(allContactsViewModel.Items);
            }
            else
            {

                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult CreateContact()
        {
            ViewData["StatesData"] = stateList;
            return View();
        }

        [HttpPost]
        public IActionResult CreateContact(UserContact userContact)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BadRequest"] = "Model is not valid. Please fill all the requried fields!";
                return View();
            }
            var userProfile = this.userDomain.GetUserProfile(LoggedUser);
            userContact.UserId = userProfile.Id;
            this.contactDomain.CreateUserContact(userContact);
            return RedirectToAction("AllContactsView");
        }

        public IActionResult EditContact(int id)
        {
            UserContact userContact = null;
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                userContact = this.contactDomain.GetUserContact(id);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var item in stateList)
            {
                if (item.Value == userContact.State)
                {
                    item.Selected = true;
                    break;
                }
            }
            ViewData["StatesData"] = stateList;
            return View(userContact);
        }

        [HttpPost]
        public IActionResult EditContact(UserContact userContact)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BadRequest"] = "Model is not valid. Please fill all the requried fields!";
                return View();
            }
            this.contactDomain.UpdateUserContact(userContact);
            return RedirectToAction("AllContactsView");
        }

        public IActionResult RemoveContact(int id)
        {
            UserContact userContact = this.contactDomain.GetUserContact(id);
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                ViewData["RemoveWarning"] = $"Are you sure you want to delete {userContact.LastName}'s contact?";
                return View();
            }
            else
            {
                return RedirectToAction("AllContactsView");
            }
        }
        [HttpPost]
        public IActionResult RemoveContact(UserContact userContact)
        {
            this.contactDomain.RemoveUserContact(userContact);
            return RedirectToAction("AllContactsView");
        }
    }
}
