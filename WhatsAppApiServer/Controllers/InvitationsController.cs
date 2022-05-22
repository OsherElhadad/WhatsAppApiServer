using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WhatsAppApiServer.Hubs;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvitationsController : ControllerBase
    {
        private readonly ContactsService _contactsService;
        private readonly HubService _hubService;
        private readonly IHubContext<MyHub> _myHub;
        public InvitationsController(ContactsService contactsService, IHubContext<MyHub> myHub, HubService hubService)
        {
            _contactsService = contactsService;
            _myHub = myHub;
            _hubService = hubService;
        }

        // POST: Invitations
        [HttpPost]
        public async Task<IActionResult> PostInvitations([Bind("From,To,Server")] Invitation invitation)
        {
            if (invitation.From == null || invitation.To == null || invitation.Server == null)
            {
                return BadRequest();
            }
            var contact = new Contact();
            contact.Id = invitation.From;
            contact.Name = invitation.From;
            contact.Server = invitation.Server;
            if (!await _contactsService.AddContact(invitation.To, contact))
            {
                return BadRequest();
            }

            string? connectionID = _hubService.GetConnectionId(invitation.To);

            if (connectionID != null)
            {
                await _myHub.Clients.Client(connectionID).SendAsync("ContactChangeRecieved", contact);
            }
            return Created(nameof(PostInvitations), null);
        }
    }
}
