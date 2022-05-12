using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Data
{
    public class ContactsContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=WhatsAppApiDB;user=root;password=Osh841998";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>().HasKey(c => c.Id);
            modelBuilder.Entity<Contact>().HasMany(c => c.Conversations).WithOne(cv => cv.Contact);
        }

        public DbSet<Contact> Contacts { get; set; }

    }
}
