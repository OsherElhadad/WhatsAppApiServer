using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Hubs;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly HubService _hubService;
        private readonly MessagesService _messagesService;
        private readonly ContactsService _contactsService;
        private readonly IHubContext<MyHub> _myHub;
        public TransferController(MessagesService messagesService, ContactsService contactsService, IHubContext<MyHub> myHub, HubService hubService)
        {
            _messagesService = messagesService;
            _myHub = myHub;
            _contactsService = contactsService;
            _hubService = hubService;
        }

        // POST: Transfer
        [HttpPost]
        public async Task<IActionResult> PostTransfer([Bind("From,To,Content")] Transfer transfer)
        {
            if (transfer == null || transfer.From == null || transfer.To == null || transfer.Content == null)
            {
                return BadRequest();
            }
            var message = await _messagesService.AddMessageTransfer(transfer.To, transfer.From, transfer.Content);
            if (message == null)
            {
                return BadRequest();
            }

            var contact = await _contactsService.GetContact(transfer.To, transfer.From);

            string? connectionID = _hubService.GetConnectionId(transfer.To);

            if (connectionID != null)
            {
                await _myHub.Clients.Client(connectionID).SendAsync("MessageChangeRecieved", contact, message);
            }
            return Created(nameof(PostTransfer), null);
        }
    }
}
