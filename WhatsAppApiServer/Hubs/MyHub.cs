using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Hubs
{
    public class MyHub : Hub
    {
        public async Task MessageChanged(List<Contact>? contacts)
        {
            await Clients.All.SendAsync("MessageChangeRecieved", contacts);
        }

        public async Task ContactChanged(List<Contact>? contacts)
        {
            await Clients.All.SendAsync("ContactChangeRecieved", contacts);
        }
    }
}
