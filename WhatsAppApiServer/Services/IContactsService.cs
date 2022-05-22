using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public abstract class IContactsService
    {
        public abstract Task<List<Contact>?> GetContacts(string userId);

        public abstract Task<Contact?> GetContact(string userId, string contactId);

        public abstract Task<bool> AddContact(string userId, Contact contact);

        public abstract Task<bool> UpdateContact(string userId, string contactId, string newName, string newServer);

        public abstract Task<bool> DeleteContact(string userId, string contactId);

        public abstract Task<bool> DeleteContactsOfUser(string userId);

        public abstract bool ContactExists(string userId, string contactId);
    }
}
