using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public class UsersService
    {
        private readonly WhatsAppApiContext _context;
        public UsersService(WhatsAppApiContext usersContext)
        {
            _context = usersContext;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<bool> AddUser(User user)
        {
            if (user == null || UserExists(user.Id))
            {
                return false;
            }
            try
            {
                user.Contacts = new List<Contact>();
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateUser(string oldId, string newPass)
        {
            if (!UserExists(oldId))
            {
                return false;
            }
            try
            {
                var oldUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == oldId);
                if (oldUser == null)
                {
                    return false;
                }
                oldUser.Password = newPass;
                _context.Update(oldUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteUser(string id)
        {
            if (!UserExists(id))
            {
                return false;
            }
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return false;
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public bool UserNameAndPassExists(string id, string pass)
        {
            return _context.Users.Any(u => u.Id == id && u.Password == pass);
        }
    }
}
