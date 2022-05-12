using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Data
{
    public class UsersContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=WhatsAppApiDB;user=root;password=Osh841998";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.Conversations).WithOne(cv => cv.User);
        }

        public DbSet<User> Users { get; set; }
    }
}
