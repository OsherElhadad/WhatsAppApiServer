using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public class ContactsService
    {
        private readonly WhatsAppApiContext _context;
        public ContactsService(WhatsAppApiContext contactsContext)
        {
            _context = contactsContext;
        }

        public async Task<List<Contact>?> GetContacts(string userId)
        {
            var contacts = await _context.Contacts.ToListAsync();

            if (contacts == null)
            {
                return null;
            }

            var userContacts = from contact in contacts where contact.UserId == userId select contact;

            return userContacts.ToList();
        }

        public async Task<Contact?> GetContact(string userId, string contactId)
        {
            var usersContact = await GetContacts(userId);
            
            if (usersContact == null)
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

        public async Task<bool> AddContact(string userId, Contact contact)
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
                contact.LastDate = null;
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

        public async Task<bool> UpdateContact(string userId, string contactId, string newName, string newServer)
        {
            if (!ContactExists(userId, contactId))
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

        public async Task<bool> DeleteContact(string userId, string contactId)
        {
            if (!ContactExists(userId, contactId))
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

        public bool ContactExists(string userId, string contactId)
        {
            return _context.Contacts.Any(c => c.Id == contactId && c.UserId == userId);
        }
    }
}
