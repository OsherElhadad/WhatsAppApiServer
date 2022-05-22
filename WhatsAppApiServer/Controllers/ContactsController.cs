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
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService _service;
        private readonly HubService _hubService;
        private readonly IHubContext<MyHub> _myHub;

        public ContactsController(ContactsService contactsService, IHubContext<MyHub> myHub, HubService hubService)
        {
            _service = contactsService;
            _myHub = myHub;
            _hubService = hubService;
        }

        // GET: Contacts
        [HttpGet(Name = "GetContacts")]
        public async Task<IActionResult> GetContacts()
        {
            string? current = getCurrentLogedUser();

            if (current == null)
            {
                return Unauthorized();
            }

            var userContacts = await _service.GetContacts(current);
            
            if (userContacts == null)
            {
                return NotFound();
            }

            return Ok(userContacts);
        }

        // GET: Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContacts(string id)
        {
            string? current = getCurrentLogedUser();

            if (current == null)
            {
                return Unauthorized();
            }
            var userContact = await _service.GetContact(current, id);

            if (userContact == null)
            {
                return NotFound();
            }

            return Ok(userContact);
        }

        // POST: Contacts
        [HttpPost]
        public async Task<IActionResult> PostContacts([Bind("Id,Name,Server")] Contact contact)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (ModelState.IsValid)
            {
                if (!await _service.AddContact(current, contact))
                {
                    return BadRequest();
                }

                string? connectionID = _hubService.GetConnectionId(current);

                if (connectionID != null)
                {
                    await _myHub.Clients.Client(connectionID).SendAsync("ContactChangeRecieved", contact);
                }
            }
            return Created(nameof(PostContacts), null);
        }

        // PUT: Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts(string id, [Bind("Name,Server")] Contact contact)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _service.UpdateContact(current, id, contact.Name, contact.Server))
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacts(string id)
        {
            string? current = getCurrentLogedUser();
            if (current == null)
            {
                return Unauthorized();
            }
            if (!await _service.DeleteContact(current, id))
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
