using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;
using TelephoneDirectoryApp.Contracts;
using System;

namespace TelephoneDirectoryApp.Business
{
    public class UserDomain : IUserDomain
    {
        private IRepository repository;
        private ILogger logger;

        public UserDomain(IRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        public void CreateUserProfile(UserProfile userProfile)
        {
            try
            {
                this.repository.CreateUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception in bussiness: {ex.Message}");
            }
        }

        public UserProfile GetUserProfile(string username)
        {
            return this.repository.GetUserProfile(username);
        }

        public UserProfile GetUserProfile(int id)
        {
            return this.repository.GetUserProfile(id);
        }

        public bool ValidateUser(LoginViewModel loginViewModel)
        {
            return this.repository.ValidateUser(loginViewModel);
        }
        public AllProfilesViewModel GetAllUsers(UserSearchViewModel userSearchViewModel)
        {
            return this.repository.GetAllUsers(userSearchViewModel);
        }
        public void UpdateUserProfile(UserProfile userProfile)
        {
            this.repository.UpdateUserProfile(userProfile);
        }
        public void UpdatePassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            this.repository.UpdatePassword(resetPasswordViewModel);
        }
        public void RemoveUserProfile(UserProfile userProfile)
        {
            try
            {
                this.repository.RemoveUserProfile(userProfile);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception in bussiness: {ex.Message}");
            }
        }
    }
}

