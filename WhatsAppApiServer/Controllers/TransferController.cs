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
        public async Task<IActionResult> PostTransfer(string from, string to, string content)
        {
            if (from == null || to == null || content == null)
            {
                return BadRequest();
            }
            if (!await _messagesService.AddMessageTransfer(from, to, content))
            {
                return BadRequest();
            }

            return Created(nameof(PostTransfer), null);
        }
    }
}
