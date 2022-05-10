using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersContext _context;

        public UsersController(UsersContext context)
        {
            _context = context;
        }

        // GET: Users
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(await _context.Users.ToListAsync());
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(string? id)
        {
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // POST: Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([Bind("Id,Password")] User user)
        {
            if (UserExists(user.Id))
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                user.Contacts = new List<Contact>();
                _context.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
            }
            return BadRequest();
        }

        // PUT: Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(string? id, [Bind("Password")] User user)
        {
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            if (user == null || user.Password == null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var oldUser = await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == id);
                    if (oldUser == null)
                    {
                        return NotFound();
                    }
                    oldUser.Password = user.Password;
                    _context.Update(oldUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
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

        // DELETE: Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null || !UserExists(id))
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }
    }
}
