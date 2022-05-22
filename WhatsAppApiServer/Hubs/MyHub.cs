using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Hubs
{
    public class MyHub : Hub
    {
        private readonly HubService _hubService;
        public MyHub(HubService hubService) { 
            _hubService = hubService;
        }
        public async Task Connect(string username)
        {
            _hubService.AddUserConnection(username, Context.ConnectionId);
        }
    }
}
