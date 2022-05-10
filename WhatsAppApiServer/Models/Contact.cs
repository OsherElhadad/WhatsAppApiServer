using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhatsAppApiServer.Models
{
    public class Contact
    {
        [Key, Column(Order = 0)]
        public string? Id { get; set; }

        [Key, Column(Order = 1)]
        [Required]
        [JsonIgnore]
        public User User { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s*$).+$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(?!\s*$).+$")]
        public string Server { get; set; }

        public string? Last { get; set; }

        public DateTime? Lastdate { get; set; }

        [JsonIgnore]
        public List<Message>? Messages { get; set; }

    }
}
