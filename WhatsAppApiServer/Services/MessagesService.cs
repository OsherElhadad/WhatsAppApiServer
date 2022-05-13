using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public class MessagesService
    {
        private readonly WhatsAppApiContext _context;
        public MessagesService(WhatsAppApiContext contactsContext)
        {
            _context = contactsContext;
        }

        public async Task<List<Message>?> GetMessages(string userId, string contactId)
        {
            var messages = await _context.Messages.ToListAsync();
            if (messages == null)
            {
                return null;
            }

            var userContactMessages = from message in messages where message.Contact.UserId == userId && message.Contact.Id == contactId select message;

            return userContactMessages.ToList();
        }

        public async Task<Message?> GetMessage(string userId, string contactId, int messageId)
        {
            var userContactMessages = await GetMessages(userId, contactId);

            if (userContactMessages == null)
            {
                return null;
            }

            var userContactMessage = from message in userContactMessages where message.Id == messageId select message;

            if (userContactMessage == null)
            {
                return null;
            }

            return userContactMessage.FirstOrDefault();
        }

        public async Task<bool> AddMessage(string userId, string contactId, string content)
        {
            try
            {
                var contact = await _context.Contacts.FirstOrDefaultAsync(u => u.Id == contactId && u.UserId == userId);
                if (contact == null)
                {
                    return false;
                }
                var message = new Message();
                message.Created = DateTime.Now;
                message.Content = content;
                message.Sent = false;
                contact.Last = content;
                contact.LastDate = message.Created;
                message.Contact = contact;

                if (contact.Messages == null)
                {
                    contact.Messages = new List<Message>();
                }
                contact.Messages.Add(message);
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateMessage(string userId, string contactId, int messageId, string content)
        {
            if (!MessageExists(userId, contactId, messageId))
            {
                return false;
            }
            try
            {
                var oldMessage = await GetMessage(userId, contactId, messageId);
                if (oldMessage == null)
                {
                    return false;
                }
                oldMessage.Created = DateTime.Now;
                oldMessage.Contact.Last = content;
                oldMessage.Contact.LastDate = oldMessage.Created;

                _context.Update(oldMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteMessage(string userId, string contactId, int messageId)
        {
            if (!MessageExists(userId, contactId, messageId))
            {
                return false;
            }
            try
            {
                var userContactMessage = await GetMessage(userId, contactId, messageId);
                if (userContactMessage == null)
                {
                    return false;
                }
                if (userContactMessage.Contact.Messages != null)
                {
                    userContactMessage.Contact.Messages.Remove(userContactMessage);
                }
                _context.Messages.Remove(userContactMessage);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private bool MessageExists(string userId, string contactId, int messageId)
        {
            return _context.Messages.Any(m => m.Id == messageId && m.Contact.Id == contactId && m.Contact.UserId == userId);
        }
    }
}
