using TelephoneDirectoryApp.Entity;
using TelephoneDirectoryApp.Entity.ViewModels;
using TelephoneDirectoryApp.Contracts;
using System;
namespace TelephoneDirectoryApp.Business
{
    public class ContactDomain : IContactDomain
    {
        private IRepository repository;
        private ILogger logger;

        public ContactDomain(IRepository repository, ILogger logger)
        {
            this.repository = repository;
            this.logger = logger;
        }
        public void CreateUserContact(UserContact userContact)
        {
            try
            {
                this.repository.CreateUserContact(userContact);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception in bussiness: {ex.Message}");
            }
        }

        public AllContactsViewModel GetAllContacts(ContactSearchViewModel contactSearchViewModel)
        {
            return this.repository.GetAllContacts(contactSearchViewModel);
        }

        public void RemoveUserContact(UserContact userContact)
        {
            try
            {
                this.repository.RemoveUserContact(userContact);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Exception in bussiness: {ex.Message}");
            }
        }

        public void UpdateUserContact(UserContact userContact)
        {
            this.repository.UpdateUserContact(userContact);
        }
        public UserContact GetUserContact(int id)
        {
            return this.repository.GetUserContact(id);
        }
    }
}
