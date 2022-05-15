using Microsoft.AspNetCore.Mvc;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly MessagesService _messagesService;
        public TransferController(MessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        // POST: Transfer
        [HttpPost]
        public async Task<IActionResult> PostTransfer([Bind("From,To,Content")] Transfer transfer)
        {
            if (transfer.From == null || transfer.To == null || transfer.Content == null)
            {
                return BadRequest();
            }
            if (!await _messagesService.AddMessageTransfer(transfer.To, transfer.From, transfer.Content))
            {
                return BadRequest();
            }

            return Created(nameof(PostTransfer), null);
        }
    }
}
