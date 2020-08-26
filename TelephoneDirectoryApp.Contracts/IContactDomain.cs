using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;

namespace TelephoneDirectoryApp.Contracts
{
    public interface IContactDomain
    {
        void CreateUserContact(UserContact userContact);
        AllContactsViewModel GetAllContacts(ContactSearchViewModel contactSearchViewModel);
        void UpdateUserContact(UserContact userContact);
        void RemoveUserContact(UserContact userContact);
        UserContact GetUserContact(int id);
    }
}
