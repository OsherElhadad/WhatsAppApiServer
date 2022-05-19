using WhatsAppApiServer.Data;

namespace WhatsAppApiServer.Services
{
    public class HubService
    {
        private readonly WhatsAppApiContext _context;
        private static Dictionary<string, string> _users = new Dictionary<string, string>();

        public HubService(WhatsAppApiContext context)
        {
            _context = context;
        }

        public void AddUserConnection(string username, string connectionId)
        {
            _users[username] = connectionId;
        }

        public string? GetConnectionId(string username)
        {
            return _users.GetValueOrDefault(username);
        }
    }
}
