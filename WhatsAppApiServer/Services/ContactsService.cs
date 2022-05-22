using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public class ContactsService : IContactsService
    {
        private readonly WhatsAppApiContext _context;
        private readonly IMessagesService _service;
        public ContactsService(WhatsAppApiContext contactsContext, MessagesService service)
        {
            _context = contactsContext;
            _service = service;
        }

        public override async Task<List<Contact>?> GetContacts(string userId)
        {
            var contacts = await _context.Contacts.ToListAsync();

            if (contacts == null || userId == null)
            {
                return null;
            }

            var userContacts = from contact in contacts where contact.UserId == userId select contact;

            return userContacts.ToList();
        }

        public override async Task<Contact?> GetContact(string userId, string contactId)
        {
            var usersContact = await GetContacts(userId);
            
            if (usersContact == null || contactId == null)
            {
                return null;
            }

            var userContact = from contact in usersContact where contact.Id == contactId select contact;

            if (userContact == null)
            {
                return null;
            }

            return userContact.FirstOrDefault();
        }

        public override async Task<bool> AddContact(string userId, Contact contact)
        {
            if (contact == null || contact.Id == null || ContactExists(userId, contact.Id))
            {
                return false;
            }
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return false;
                }
                contact.UserId = userId;
                contact.User = user;
                contact.Messages = new List<Message>();
                contact.Last = null;
                contact.Lastdate = null;
                if (user.Contacts == null)
                {
                    user.Contacts = new List<Contact>();
                }
                user.Contacts.Add(contact);
                _context.Users.Update(user);
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override async Task<bool> UpdateContact(string userId, string contactId, string newName, string newServer)
        {
            if (userId == null || contactId == null || newName == null ||
                newServer == null || !ContactExists(userId, contactId))
            {
                return false;
            }
            try
            {
                var oldContact = await GetContact(userId, contactId);
                if (oldContact == null)
                {
                    return false;
                }
                oldContact.Name = newName;
                oldContact.Server = newServer;
                _context.Update(oldContact);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override async Task<bool> DeleteContact(string userId, string contactId)
        {
            if (userId == null || contactId == null || !ContactExists(userId, contactId))
            {
                return false;
            }
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return false;
                }
                var userContact = await GetContact(userId, contactId);
                if (userContact == null)
                {
                    return false;
                }
                if (user.Contacts != null && user.Contacts.Count > 0)
                {
                    user.Contacts.Remove(userContact);
                }
                if (!await _service.DeleteMessagesOfContact(userId, contactId))
                {
                    return false;
                }
                _context.Users.Update(user);
                _context.Contacts.Remove(userContact);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override async Task<bool> DeleteContactsOfUser(string userId)
        {
            if (userId == null)
            {
                return false;
            }
            var userContacts = await GetContacts(userId);
            if (userContacts == null)
            {
                return true;
            }
            foreach (var contact in userContacts)
            {
                if (!await _service.DeleteMessagesOfContact(userId, contact.Id))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool ContactExists(string userId, string contactId)
        {
            return _context.Contacts.Any(c => c.Id == contactId && c.UserId == userId);
        }
    }
}
