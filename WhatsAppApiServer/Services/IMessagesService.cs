using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public abstract class IMessagesService
    {
        public abstract Task<List<Message>?> GetMessages(string userId, string contactId);

        public abstract Task<Message?> GetMessage(string userId, string contactId, int messageId);

        public abstract Task<Message?> AddMessage(string userId, string contactId, string content);

        public abstract Task<Message?> AddMessageTransfer(string userId, string contactId, string content);

        public abstract Task<bool> UpdateMessage(string userId, string contactId, int messageId, string content);

        public abstract Task<bool> DeleteMessage(string userId, string contactId, int messageId);

        public abstract Task<bool> DeleteMessagesOfContact(string? userId, string? contactId);
        public abstract bool MessageExists(string userId, string contactId, int messageId);
    }
}
