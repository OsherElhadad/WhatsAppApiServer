using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhatsAppApiServer.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [StringLength(2000)]
        [Required]
        [RegularExpression(@"^(?!\s*$).+$")]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Created { get; set; }

        [Required]
        public bool Sent { get; set; }

        [JsonIgnore]
        public Contact? Contact { get; set; }

        [JsonIgnore]
        public string? ContactId { get; set; }

        [JsonIgnore]
        public string? UserId { get; set; }
    }
}
