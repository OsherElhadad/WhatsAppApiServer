using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Data
{
    public class ConversationsContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=WhatsAppApiDB;user=root;password=Osh841998";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conversation>().HasKey(cv => cv.Id);
            modelBuilder.Entity<Conversation>().HasMany(cv => cv.Messages).WithOne(m => m.Conversation);
            modelBuilder.Entity<Conversation>().HasOne(cv => cv.User).WithMany(u => u.Conversations);
            modelBuilder.Entity<Conversation>().HasOne(cv => cv.User).WithMany(u => u.Conversations);
        }

        public DbSet<Contact> Contacts { get; set; }

    }
}
