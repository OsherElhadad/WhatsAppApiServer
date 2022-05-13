using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly WhatsAppApiContext _context;
        public IConfiguration _configuration;
        public UsersController(WhatsAppApiContext usersContext, IConfiguration configuration)
        {
            _context = usersContext;
            _configuration = configuration;
    }

        // GET: Users
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string? id)
        {
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return Ok(user);
        }

        // POST: Users
        [HttpPost]
        public async Task<IActionResult> PostUsers([Bind("Id,Password")] User user)
        {
            if (user.Id == null || UserExists(user.Id))
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {

                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id),
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    signingCredentials: signIn);

                user.Contacts = new List<Contact>();
                _context.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest();
        }

        // PUT: Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(string? id, string? password)
        {
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            if (password == null)
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
                    oldUser.Password = password;
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
            if (id == null || !UserExists(id))
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);

            if (user == null)
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
