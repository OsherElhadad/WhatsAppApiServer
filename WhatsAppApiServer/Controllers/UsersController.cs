using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;
using WhatsAppApiServer.Services;

namespace WhatsAppApiServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _service;
        public IConfiguration _configuration;
        public UsersController(UsersService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _service.GetUsers();
            if (users == null || users.Count == 0)
            {
                return NotFound();
            }
            return Ok(users.ToList());
        }

        // GET: Users/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsers(string id)
        {
            var user = await _service.GetUser(id);
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
            if (ModelState.IsValid)
            {
                if (! await _service.AddUser(user))
                {
                    return BadRequest();
                }
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
                    expires: DateTime.UtcNow.AddMinutes(20),
                    signingCredentials: signIn);

                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
            return BadRequest();
        }

        // PUT: Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsers(string id, [Bind("Password")] User user)
        {
            if (! await _service.UpdateUser(id, user.Password))
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            if (! await _service.DeleteUser(id))
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
