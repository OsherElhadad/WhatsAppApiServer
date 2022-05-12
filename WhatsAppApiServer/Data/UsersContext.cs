using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Data
{
    public class UsersContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=WhatsAppApiDB;user=root;password=123";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the Name property as the primary
            // key of the Items table
            modelBuilder.Entity<User>().HasKey(e => e.Id);
        }

        public DbSet<User> Users { get; set; }
    }
}
