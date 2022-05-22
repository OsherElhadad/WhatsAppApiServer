using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WhatsAppApiServer.Models
{
    public class User
    {
        [Key]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]{1}[a-zA-Z0-9]+$")]
        public string? Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]{1}[a-zA-Z0-9]+$")]
        public string Password { get; set; }

        [JsonIgnore]
        public List<Contact>? Contacts { get; set; }

    }
}
