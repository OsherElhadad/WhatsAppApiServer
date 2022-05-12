using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhatsAppApiServer.Models
{
    public class Contact
    {
        [Key]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]{2}[a-zA-Z0-9]+$")]
        public string? Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^(?!\s*$).+$")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(?!\s*$).+$")]
        public string Server { get; set; }

        [JsonIgnore]
        public List<Conversation>? Conversations { get; set; }
    }
}
