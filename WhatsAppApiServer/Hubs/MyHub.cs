using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Hubs
{
    public class MyHub : Hub
    {
        public async Task Connect(string username)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, username);
        }
    }
}
