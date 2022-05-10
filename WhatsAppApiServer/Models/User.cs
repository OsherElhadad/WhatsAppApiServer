using System.ComponentModel.DataAnnotations;

namespace WhatsAppApiServer.Models
{
    public class User
    {
        [Key]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9]{2}[a-zA-Z0-9]+$")]
        public string? Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^[a-zA-Z0-9]{2}[a-zA-Z0-9]+$")]
        public string Password { get; set; }

        public List<Contact>? Contacts { get; set; }

    }
}
