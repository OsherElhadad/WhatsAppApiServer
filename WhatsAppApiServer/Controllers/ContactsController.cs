using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactsContext _context;

        public ContactsController(ContactsContext context)
        {
            _context = context;
        }

        // GET: Contacts
        [HttpGet(Name = "GetContacts")]
        public async Task<IActionResult> GetContacts()
        {
            var contact = await _context.Contacts.ToListAsync();

            if (contact == null || contact.Count == 0)
            {
                return NotFound();
            }

            return Ok(await _context.Contacts.ToListAsync());
        }

        // GET: Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContacts(string? id)
        {
            if (id == null || !ContactExists(id))
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        // POST: Contacts
        [HttpPost]
        public async Task<IActionResult> PostContacts([Bind("Id,Name,Server")] Contact contact)
        {
            if (contact == null || contact.Id == null || ContactExists(contact.Id))
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                contact.Messages = new List<Message>();
                contact.Last = null;
                contact.Lastdate = null;
                // contact.User = ??
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
            }
            return BadRequest();
        }

        // PUT: Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContacts(string? id, [Bind("Name,Server")] Contact contact)
        {
            if (id == null || !ContactExists(id))
            {
                return NotFound();
            }

            if (contact == null || contact.Name == null || contact.Server == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldContact = await _context.Contacts
                        .FirstOrDefaultAsync(c => c.Id == id);
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
                    if (!ContactExists(id))
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
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null || !ContactExists(id))
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ContactExists(string id)
        {
            return _context.Contacts.Any(c => c.Id == id);
        }
    }
}
