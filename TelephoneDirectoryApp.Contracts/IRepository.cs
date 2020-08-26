using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;

namespace TelephoneDirectoryApp.Contracts
{
    public interface IRepository
    {
        void CreateUserProfile(UserProfile userProfile);
        bool ValidateUser(LoginViewModel loginViewModel);
        UserProfile GetUserProfile(string username);
        AllProfilesViewModel GetAllUsers(UserSearchViewModel userSearchViewModel);
        void UpdateUserProfile(UserProfile userProfile);
        void UpdatePassword(ResetPasswordViewModel resetPasswordViewModel);
        UserProfile GetUserProfile(int id);
        void RemoveUserProfile(UserProfile userProfile);
        void CreateUserContact(UserContact userContact);
        AllContactsViewModel GetAllContacts(ContactSearchViewModel contactSearchViewModel);
        void UpdateUserContact(UserContact userContact);
        void RemoveUserContact(UserContact userContact);
        UserContact GetUserContact(int id);
    }
}
