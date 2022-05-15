using Microsoft.AspNetCore.Mvc;
using WhatsAppApiServer.Hubs;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly MessagesService _messagesService;
        private readonly ContactsService _contactsService;
        private readonly MyHub _myHub;
        public TransferController(MessagesService messagesService, ContactsService contactsService, MyHub myHub)
        {
            _messagesService = messagesService;
            _myHub = myHub;
            _contactsService = contactsService;
        }

        // POST: Transfer
        [HttpPost]
        public async Task<IActionResult> PostTransfer([Bind("From,To,Content")] Transfer transfer)
        {
            if (transfer == null || transfer.From == null || transfer.To == null || transfer.Content == null)
            {
                return BadRequest();
            }
            if (!await _messagesService.AddMessageTransfer(transfer.To, transfer.From, transfer.Content))
            {
                return BadRequest();
            }
            var contacts = await _contactsService.GetContacts(transfer.To);
            await _myHub.MessageChanged(contacts);

            return Created(nameof(PostTransfer), null);
        }
    }
}
