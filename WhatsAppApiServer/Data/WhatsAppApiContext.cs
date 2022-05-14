using Microsoft.EntityFrameworkCore;
using WhatsAppApiServer.Models;

namespace WhatsAppApiServer.Data
{
    public class WhatsAppApiContext : DbContext
    {
        private const string connectionString = "server=localhost;port=3306;database=WhatsAppApiDB;user=root;password=Osh841998";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, MariaDbServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<User>().HasMany(u => u.Contacts).WithOne(c => c.User).HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Contact>().HasKey(c => new { c.Id, c.UserId });
            modelBuilder.Entity<Contact>().HasOne(c => c.User).WithMany(u => u.Contacts).HasForeignKey(c => c.UserId);
            modelBuilder.Entity<Contact>().HasMany(c => c.Messages).WithOne(m => m.Contact).HasForeignKey(m => new { m.ContactId, m.UserId });

            modelBuilder.Entity<Message>().HasKey(m => m.Id);
            modelBuilder.Entity<Message>().HasOne(m => m.Contact).WithMany(c => c.Messages).HasForeignKey(m => new { m.ContactId, m.UserId });
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
