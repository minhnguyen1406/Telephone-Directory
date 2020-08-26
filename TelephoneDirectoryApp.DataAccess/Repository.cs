using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TelephoneDirectoryApp.Contracts;
using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;

namespace TelephoneDirectoryApp.DataAccess
{
    public class Repository : IRepository
    {
        private UserDbContext context;
        private ILogger logger;

        public Repository(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.context = new UserDbContext(configuration);
        }

        public string UpdateField(string entityToUpdate, string inputEntity)
        {
            string result = "";
            if (!string.IsNullOrEmpty(inputEntity))
            {
                result = inputEntity;
            }
            else
            {
                result = entityToUpdate;
            }
            return result;
        }

        #region User Profiles CRUD logic
        public void CreateUserProfile(UserProfile userProfile)
        {
            try
            {
                userProfile.CreateDt = DateTime.Now;
                userProfile.CreateBy = "System";
                this.context.UserProfiles.Add(userProfile);
                this.context.SaveChanges();
                this.logger.LogInfo("User Profile has been created!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        public UserProfile GetUserProfile(string username)
        {
            return this.context.UserProfiles.FirstOrDefault(up => up.Username == username);
        }

        public UserProfile GetUserProfile(int id)
        {
            return this.context.UserProfiles.FirstOrDefault(up => up.Id == id);
        }

        public bool ValidateUser(LoginViewModel loginViewModel)
        {
            var user = this.context.UserProfiles.FirstOrDefault(up => up.Username == loginViewModel.Username && up.Password == loginViewModel.Password);
            return user != null ? true : false;
        }


        public void UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                var userToUpdate = GetUserProfile(userProfile.Username);
                userToUpdate.Username = userProfile.Username;
                userToUpdate.FirstName = UpdateField(userToUpdate.FirstName, userProfile.FirstName);
                userToUpdate.LastName = UpdateField(userToUpdate.LastName, userProfile.LastName);
                userToUpdate.Email = UpdateField(userToUpdate.Email, userProfile.Email);
                userToUpdate.UpdateDt = DateTime.Now;
                userToUpdate.UpdateBy = userProfile.Username;
                this.context.SaveChanges();
                this.logger.LogInfo("User Profile has been updated!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        public void UpdatePassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            var user = GetUserProfile(resetPasswordViewModel.Username);
            user.Username = resetPasswordViewModel.Username;
            user.Password = resetPasswordViewModel.NewPassword;
            this.context.SaveChanges();
        }

        public AllProfilesViewModel GetAllUsers(UserSearchViewModel userSearchViewModel)
        {
            var outputModel = new AllProfilesViewModel();
            outputModel.PageIndex = userSearchViewModel.PageIndex;
            var allUsers = this.context.UserProfiles.ToList();
            if (userSearchViewModel != null)
            {
                if (!string.IsNullOrEmpty(userSearchViewModel.UsernameSearch))
                {
                    allUsers = allUsers.Where(u => u.Username.Contains(userSearchViewModel.UsernameSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(userSearchViewModel.FirstNameSearch))
                {
                    allUsers = allUsers.Where(u => u.FirstName.Contains(userSearchViewModel.FirstNameSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(userSearchViewModel.LastNameSearch))
                {
                    allUsers = allUsers.Where(u => u.LastName.Contains(userSearchViewModel.LastNameSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(userSearchViewModel.EmailSearch))
                {
                    allUsers = allUsers.Where(u => u.Email.Contains(userSearchViewModel.EmailSearch)).ToList();
                }
            }
            outputModel.TotalItems = allUsers.Count;
            if (userSearchViewModel.PageSize == 0)
            {
                outputModel.Items = allUsers;
            }
            if (userSearchViewModel != null && userSearchViewModel.PageIndex > 0 && userSearchViewModel.PageSize > 0)
            {
                outputModel.Items = allUsers.Skip((userSearchViewModel.PageIndex - 1) * userSearchViewModel.PageSize).Take(userSearchViewModel.PageSize).ToList();
            }
            return outputModel;

        }
        public void RemoveUserProfile(UserProfile userProfile)
        {
            try
            {
                this.context.UserContacts.RemoveRange(this.context.UserContacts.Where(uc => uc.UserId == userProfile.Id));
                this.context.UserProfiles.Remove(userProfile);
                this.context.SaveChanges();
                this.logger.LogInfo("User Profile has been removed!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }
        #endregion

        #region User Contacts CRUD logic
        public void CreateUserContact(UserContact userContact)
        {
            
            try
            {
                this.context.UserContacts.Add(userContact);
                this.context.SaveChanges();
                this.logger.LogInfo("User Contact has added!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        public AllContactsViewModel GetAllContacts(ContactSearchViewModel contactSearchViewModel)
        {
            var outputModel = new AllContactsViewModel();
            outputModel.PageIndex = contactSearchViewModel.PageIndex;
            var allContacts = this.context.UserContacts.Include(uc => uc.UserProfile).Where(uc => uc.UserProfile.Id == contactSearchViewModel.UserId).ToList();
            if (contactSearchViewModel != null)
            {
                if (!string.IsNullOrEmpty(contactSearchViewModel.FirstNameSearch))
                {
                    allContacts = allContacts.Where(uc => uc.FirstName.Contains(contactSearchViewModel.FirstNameSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(contactSearchViewModel.LastNameSearch))
                {
                    allContacts = allContacts.Where(uc => uc.LastName.Contains(contactSearchViewModel.LastNameSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(contactSearchViewModel.PhoneSearch))
                {
                    allContacts = allContacts.Where(uc => uc.Phone.Contains(contactSearchViewModel.PhoneSearch)).ToList();
                }
            }
            outputModel.TotalItems = allContacts.Count;
            if (contactSearchViewModel.PageSize == 0)
            {
                outputModel.Items = allContacts;
            }
            if (contactSearchViewModel != null && contactSearchViewModel.PageIndex > 0 && contactSearchViewModel.PageSize > 0)
            {
                outputModel.Items = allContacts.Skip((contactSearchViewModel.PageIndex - 1) * contactSearchViewModel.PageSize).Take(contactSearchViewModel.PageSize).ToList();
            }
            return outputModel;
        }

        public UserContact GetUserContact (int id)
        {
            return this.context.UserContacts.FirstOrDefault(uc => uc.Id == id);
        }

        public void UpdateUserContact(UserContact userContact)
        {
            var contactToUpdate = GetUserContact(userContact.Id);
            try
            {
                contactToUpdate.FirstName = UpdateField(contactToUpdate.FirstName, userContact.FirstName);
                contactToUpdate.LastName = UpdateField(contactToUpdate.LastName, userContact.LastName);
                contactToUpdate.Email = UpdateField(contactToUpdate.Email, userContact.Email);
                contactToUpdate.Gender = UpdateField(contactToUpdate.Gender, userContact.Gender);
                contactToUpdate.Phone = UpdateField(contactToUpdate.Phone, userContact.Phone);
                contactToUpdate.City = UpdateField(contactToUpdate.City, userContact.City);
                contactToUpdate.State = UpdateField(contactToUpdate.State, userContact.State);
                contactToUpdate.Zip = UpdateField(contactToUpdate.Zip, userContact.Zip);
                this.context.SaveChanges();
                this.logger.LogInfo("User Contact has been updated!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }

        public void RemoveUserContact(UserContact userContact)
        {
            try
            {
                this.context.UserContacts.Remove(userContact);
                this.context.SaveChanges();
                this.logger.LogInfo("User Contact has been removed!");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message);
            }
        }
        #endregion
    }
}
