using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Hubs;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Contacts/{id}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly HubService _hubService;
        private readonly IMessagesService _messagesService;
        private readonly IContactsService _contactsService;
        private readonly IHubContext<MyHub> _myHub;

        public MessagesController(MessagesService messagesService, ContactsService contactsService, IHubContext<MyHub> myHub, HubService hubService)
        {
            _messagesService = messagesService;
            _contactsService = contactsService;
            _myHub = myHub;
            _hubService = hubService;
        }

        // GET: Messages
        [HttpGet(Name = "GetMessages")]
        public async Task<IActionResult> GetMessages(string id)
        {
            string? current = getCurrentLogedUser();

            if (current == null)
            {
                return Unauthorized();
            }

            var userContactMessages = await _messagesService.GetMessages(current, id);

            if (userContactMessages == null)
            {
                return NotFound();
            }

            return Ok(userContactMessages);
        }

        // GET: Messages/5
        [HttpGet("{id2}")]
        public async Task<IActionResult> GetMessages(string id, int id2)
        {
            string? current = getCurrentLogedUser();

            if (current == null)
            {
                return Unauthorized();
            }

            var userContactMessage = await _messagesService.GetMessage(current, id, id2);

            if (userContactMessage == null)
            {
                return NotFound();
            }

            return Ok(userContactMessage);
        }

        // POST: Messages
        [HttpPost]
        public async Task<IActionResult> PostMessages(string id, [Bind("Content")] Message message)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            var newMessage = await _messagesService.AddMessage(current, id, message.Content);
            if (newMessage == null)
            {
                return BadRequest();
            }
            newMessage.Sent = true;
            if (newMessage == null)
            {
                return BadRequest();
            }

            var contact = await _contactsService.GetContact(current, id);

            string? connectionID = _hubService.GetConnectionId(current);

            if (connectionID != null)
            {
                await _myHub.Clients.Client(connectionID).SendAsync("MessageChangeRecieved", contact, newMessage);
            }
            return Created(nameof(PostMessages), null);
        }

        // PUT: Messages/5
        [HttpPut("{id2}")]
        public async Task<IActionResult> PutMessages(string id, int id2, [Bind("Content")] Message message)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _messagesService.UpdateMessage(current, id, id2, message.Content))
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: Messages/5
        [HttpDelete("{id2}")]
        public async Task<IActionResult> DeleteMessages(string id, int id2)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _messagesService.DeleteMessage(current, id, id2))
            {
                return NotFound();
            }
            return NoContent();
        }

        private string? getCurrentLogedUser()
        {
            var userId = User.FindFirst("Id")?.Value;
            return userId;
        }
    }
}
