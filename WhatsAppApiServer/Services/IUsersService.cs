using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Services
{
    public abstract class IUsersService
    {
        public abstract Task<List<User>> GetUsers();

        public abstract Task<User?> GetUser(string id);

        public abstract Task<bool> AddUser(User user);

        public abstract Task<bool> UpdateUser(string oldId, string newPass);

        public abstract Task<bool> DeleteUser(string id);

        public abstract bool UserExists(string id);

        public abstract bool UserNameAndPassExists(string id, string pass);
    }
}
