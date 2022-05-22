using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Data;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public class UsersService : IUsersService
    {
        private readonly WhatsAppApiContext _context;
        private readonly ContactsService _service;
        public UsersService(WhatsAppApiContext usersContext, ContactsService service)
        {
            _context = usersContext;
            _service = service;
        }

        public override async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public override async Task<User?> GetUser(string id)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public override async Task<bool> AddUser(User user)
        {
            if (user == null || user.Id == null || UserExists(user.Id))
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

        public override async Task<bool> UpdateUser(string oldId, string newPass)
        {
            if (oldId == null || newPass == null || !UserExists(oldId))
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

        public override async Task<bool> DeleteUser(string id)
        {
            if (id == null || !UserExists(id))
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
                if (!await _service.DeleteContactsOfUser(id))
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

        public override bool UserExists(string id)
        {
            return _context.Users.Any(u => u.Id == id);
        }

        public override bool UserNameAndPassExists(string id, string pass)
        {
            return _context.Users.Any(u => u.Id == id && u.Password == pass);
        }
    }
}
