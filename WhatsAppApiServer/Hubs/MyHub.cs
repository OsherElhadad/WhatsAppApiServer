using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Hubs
{
    public class MyHub : Hub
    {
        public async Task MessageChanged(Transfer transfer)
        {
            await Clients.All.SendAsync("MessageChangeRecieved", transfer.From, transfer.To, transfer.Content);
        }

        public async Task ContactChanged(Invitation invitation)
        {
            await Clients.All.SendAsync("ContactChangeRecieved", invitation.From, invitation.To, invitation.Server);
        }
    }
}
