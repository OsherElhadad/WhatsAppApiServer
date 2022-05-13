using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/Contacts/{id}[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly WhatsAppApiContext _context;

        public MessagesController(WhatsAppApiContext context)
        {
            _context = context;
        }

        /*// GET: Messages
        [HttpGet(Name = "GetMessages")]
        public async Task<IActionResult> GetMessages(string? id)
        {
            var message = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id).Result.Messages.ToListAsync();

            if (message == null || message.Count == 0)
            {
                return NotFound();
            }

            return Ok(await _context.Messages.ToListAsync());
        }

        // GET: Messages/5
        [HttpGet("{id2}")]
        public async Task<IActionResult> GetMessages(string? id, int? id2)
        {
            if (!MessageExists(id))
            {
                return NotFound();
            }

            var message = await _context.Messages
                .FirstOrDefaultAsync(c => c.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }

        // POST: Messages
        [HttpPost]
        public async Task<IActionResult> PostMessages(string? id, [Bind("Content")] Message message)
        {
            if (message == null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                // message.Id=??
                message.Created = DateTime.Now;
                message.Sent = true;
                // message.User=??
                _context.Add(message);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetMessages), new { id = message.Id }, message);
            }
            return BadRequest();
        }

        // PUT: Messages/5
        [HttpPut("{id2}")]
        public async Task<IActionResult> PutMessages(string? id, int? id2, [Bind("Content")] Message contact)
        {
            if (id == null || !MessageExists(id))
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
                    var oldContact = await _context.Messages
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
                    if (!MessageExists(id))
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

        // DELETE: Messages/5
        [HttpDelete("{id2}")]
        public async Task<IActionResult> DeleteMessages(string? id, int? id2)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Messages.FindAsync(id);

            if (contact == null || !MessageExists(id))
            {
                return NotFound();
            }

            _context.Messages.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool MessageExists(int? id)
        {
            if (id == null)
            {
                return false;
            }
            return _context.Messages.Any(e => e.Id == id);
        }*/
    }
}
