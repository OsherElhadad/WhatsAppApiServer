using Microsoft.AspNetCore.Mvc;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly ContactsService _contactsService;
        public InvitationsController(ContactsService contactsService)
        {
            _contactsService = contactsService;
        }

        // POST: Invitations
        [HttpPost]
        public async Task<IActionResult> PostInvitations(string from, string to, string server)
        {
            if (from == null || to == null || server == null)
            {
                return BadRequest();
            }
            var contact = new Contact();
            contact.Id = to;
            contact.Name = to;
            contact.Server = server;
            if (!await _contactsService.AddContact(from, contact))
            {
                return BadRequest();
            }

            return Created(nameof(PostInvitations), null);
        }
    }
}
