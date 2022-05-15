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
        private readonly MyHub _myHub;
        public TransferController(MessagesService messagesService, MyHub myHub)
        {
            _messagesService = messagesService;
            _myHub = myHub;
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
            await _myHub.MessageChanged(transfer);

            return Created(nameof(PostTransfer), null);
        }
    }
}
