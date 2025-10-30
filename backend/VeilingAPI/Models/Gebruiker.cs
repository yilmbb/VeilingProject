using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeilingAPI.Models
{
    public abstract class Gebruiker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int gebruiker_id { get; set; }

        [Required]
        [StringLength(255)]
        public string wachtwoord { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string email { get; set; } = string.Empty;

        [Required]
        public DateTime registreren { get; set; } = DateTime.Now;

        public DateTime? inloggen { get; set; }

        // Computed property: Username derived from email
        [NotMapped]
        public string Username => email?.Split('@')[0] ?? string.Empty;
    }
}
