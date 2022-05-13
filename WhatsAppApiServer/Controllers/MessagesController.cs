using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Contacts/{id}/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesService _service;

        public MessagesController(MessagesService messagesService)
        {
            _service = messagesService;
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

            var userContactMessages = await _service.GetMessages(current, id);

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

            var userContactMessage = await _service.GetMessage(current, id, id2);

            if (userContactMessage == null)
            {
                return NotFound();
            }

            return Ok(userContactMessage);
        }

        // POST: Messages
        [HttpPost]
        public async Task<IActionResult> PostMessages(string id, string content)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _service.AddMessage(current, id, content))
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(PostMessages), null, null);
        }

        // PUT: Messages/5
        [HttpPut("{id2}")]
        public async Task<IActionResult> PutMessages(string id, int id2, string content)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _service.UpdateMessage(current, id, id2, content))
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
            if (!await _service.DeleteMessage(current, id, id2))
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
