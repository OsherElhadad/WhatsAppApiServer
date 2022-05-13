using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly WhatsAppApiContext _context;

        public ContactsController(WhatsAppApiContext context)
        {
            _context = context;
        }

        // GET: Contacts
        [HttpGet(Name = "GetContacts")]
        public async Task<IActionResult> GetContacts()
        {
            string? current = getCurrentLogedUser();
            var contacts = await _context.Contacts.ToListAsync();

            if (contacts == null || current == null)
            {
                return NotFound();
            }

            var userContacts = from contact in contacts where contact.UserId == current select contact;

            return Ok(contacts);
        }

        // GET: Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContacts(string? id)
        {
            string? current = getCurrentLogedUser();
            var contacts = await _context.Contacts.ToListAsync();
            if (contacts == null || current == null || id == null || !ContactExists(id, current))
            {
                return NotFound();
            }

            var userContact = from contact in contacts where contact.UserId == current && contact.Id == id select contact;

            return Ok(userContact.FirstOrDefault());
        }

        // POST: Contacts
        [HttpPost]
        public async Task<IActionResult> PostContacts([Bind("Id,Name,Server")] Contact contact)
        {
            string? current = getCurrentLogedUser();
            if (contact == null || current == null || contact.Id == null || contact.Name == null || contact.Server == null)
            {
                return BadRequest();
            }
            if (ContactExists(contact.Id, current))
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == current);
                if (user == null)
                {
                    return BadRequest();
                }
                contact.UserId = current;
                contact.User = user;
                contact.Messages = new List<Message>();
                contact.Last = null;
                contact.LastDate = null;
                user.Contacts.Add(contact);
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(PostContacts), null, contact);
            }
            return BadRequest();
        }

        // PUT: Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts(string? id, [Bind("Name,Server")] Contact contact)
        {
            string? current = getCurrentLogedUser();
            if (contact == null || current == null || id == null || contact.Name == null || contact.Server == null)
            {
                return BadRequest();
            }
            if (!ContactExists(id, current))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldContact = await _context.Contacts
                        .FirstOrDefaultAsync(c => c.Id == id && c.UserId == current);
                    if (oldContact == null)
                    {
                        return NotFound();
                    }
                    oldContact.Name = contact.Name;
                    oldContact.Server = contact.Server;
                    _context.Update(oldContact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(id, current))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return NoContent();
            }
            return BadRequest();
        }

        // DELETE: Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContacts(string? id)
        {
            string? current = getCurrentLogedUser();
            var contacts = await _context.Contacts.ToListAsync();
            if (contacts == null || current == null || id == null || !ContactExists(id, current))
            {
                return NotFound();
            }

            var userContact = (from contact in contacts where contact.UserId == current && contact.Id == id select contact).FirstOrDefault();
            if (userContact == null)
            {
                return NotFound();
            }

            _context.Contacts.Remove(userContact);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private string? getCurrentLogedUser()
        {
            var userId = User.FindFirst("Id")?.Value;
            return userId;
        }

        private bool ContactExists(string id, string userId)
        {
            return _context.Contacts.Any(c => c.Id == id && c.UserId == userId);
        }
    }
}
