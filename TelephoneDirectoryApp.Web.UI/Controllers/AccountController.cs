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
using System.Security.Claims;

namespace TelephoneDirectoryApp.Web.UI.Controllers
{
    public class AccountController : Controller
    {
        private IUserDomain userDomain;

        private ILogger logger;

        public string LoggedUser
        {
            get
            {
                return User?.Identity?.Name;
            }
        }

        public AccountController(IUserDomain userDomain, ILogger logger)
        {
            this.userDomain = userDomain;
            this.logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["UnAuthorizedMessage"] = "Please enter credentails and try again!";
                this.logger.LogWarning("You didn't enter credentials");
                return View();
            }

            var isValid = this.userDomain.ValidateUser(loginViewModel);
            if (isValid)
            {
                this.logger.LogInfo("You logged!");

                var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, loginViewModel.Username),
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                if (loginViewModel.Username == "admin")
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
                }
                else
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, "User"));
                }

                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["UnAuthorizedMessage"] = "Your credentails are invalid, Please try again!";
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Signup()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Signup(UserProfile userProfile)
        {
            if (!ModelState.IsValid)
            {
                ViewData["BadRequest"] = "Model is not valid. Please fill all the requried fields!";
                return View();
            }
            this.userDomain.CreateUserProfile(userProfile);
            return RedirectToAction("Login");
        }

        public IActionResult ProfileView(int? id)
        {
            ViewData["Title"] = "My Profile";
            UserProfile userProfile = null;

            if (id != null && id > 0 && User.IsInRole("Admin"))
            {
                userProfile = this.userDomain.GetUserProfile(id.Value);
            }
            else
            {

                if (!string.IsNullOrEmpty(LoggedUser))
                {
                    userProfile = this.userDomain.GetUserProfile(LoggedUser);
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            return View(userProfile);
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AllProfilesView(UserSearchViewModel userSearchViewModel, int pageIndex = 0)
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
                var userSearchDisplayViewModel = new UserSearchViewModel()
                {
                    UsernameSearch = userSearchViewModel.UsernameSearch,
                    FirstNameSearch = userSearchViewModel.FirstNameSearch,
                    LastNameSearch = userSearchViewModel.LastNameSearch,
                    EmailSearch = userSearchViewModel.EmailSearch,
                    PageIndex = pageIndex > 0 ? pageIndex : 1,
                    PageSize = Convert.ToInt32(userSearchViewModel.PageSize)
                };

                ViewData["pageIndex"] = userSearchDisplayViewModel.PageIndex;
                ViewData["pageSize"] = userSearchDisplayViewModel.PageSize;

                var allProfilesViewModel = this.userDomain.GetAllUsers(userSearchDisplayViewModel);
                ViewData["totalItems"] = allProfilesViewModel.TotalItems;
                return View(allProfilesViewModel.Items);
            }
            else
            {

                return RedirectToAction("Login");
            }
        }

        public IActionResult EditProfile()
        {
            UserProfile userProfile = null;
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                userProfile = this.userDomain.GetUserProfile(LoggedUser);
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(userProfile);
        }

        [HttpPost]
        public IActionResult EditProfile(UserProfile userProfile)
        {

            this.userDomain.UpdateUserProfile(userProfile);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPassword()
        {
            UserProfile userProfile = null;
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                userProfile = this.userDomain.GetUserProfile(LoggedUser);
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View(userProfile);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            UserProfile userProfile = null;

            userProfile = this.userDomain.GetUserProfile(LoggedUser);
            if (userProfile.Password != resetPasswordViewModel.OldPassword)
            {
                ViewData["UnAuthorizedMessage"] = "Invalid old password, Please try again!";
                return View(userProfile);
            }
            else if (resetPasswordViewModel.NewPassword == null)
            {
                ViewData["UnAuthorizedMessage"] = "New password cannot be blank";
                return View(userProfile);
            }
            else
            {
                this.userDomain.UpdatePassword(resetPasswordViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RemoveProfile(int id)
        {
            UserProfile userProfile = this.userDomain.GetUserProfile(id);
            if (!string.IsNullOrEmpty(LoggedUser))
            {
                ViewData["RemoveWarning"] = $"Are you sure you want to delete {userProfile.Username} user?";
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult RemoveProfile(UserProfile userProfile)
        {
            this.userDomain.RemoveUserProfile(userProfile);
            return RedirectToAction("AllProfilesView");
        }
    }
}
