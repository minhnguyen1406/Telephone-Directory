using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;

namespace TelephoneDirectoryApp.Contracts
{
    public interface IUserDomain
    {
        void CreateUserProfile(UserProfile userProfile);
        UserProfile GetUserProfile(string username);
        bool ValidateUser(LoginViewModel loginViewModel);
        AllProfilesViewModel GetAllUsers(UserSearchViewModel userSearchViewModel);
        void UpdateUserProfile(UserProfile userProfile);
        void UpdatePassword(ResetPasswordViewModel resetPasswordViewModel);
        UserProfile GetUserProfile(int id);
        void RemoveUserProfile(UserProfile userProfile);
        }
    }
