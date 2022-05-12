using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhatsAppApiServer.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public User User { get; set; }
        
        [Required]
        public Contact Contact { get; set; }

        public List<Message>? Messages { get; set; }

        public string? Last { get; set; }

        public DateTime? Lastdate { get; set; }
    }
}
