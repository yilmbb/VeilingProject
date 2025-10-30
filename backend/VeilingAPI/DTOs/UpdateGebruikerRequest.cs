using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeilingAPI.DTOs
{
    public class UpdateGebruikerRequest
    {
        [Required]
        public int gebruiker_id { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string wachtwoord { get; set; } = string.Empty;

        [Required]
        public string bedrijfsnaam { get; set; } = string.Empty;

        [Required]
        public string kvk { get; set; } = string.Empty;

        [Required]
        public string btw_nummer { get; set; } = string.Empty;

        [Required]
        public string telefoon { get; set; } = string.Empty;

        [Required]
        public string rekening_nummer { get; set; } = string.Empty;

        public string? bedrijfsadres { get; set; }
        public string? leveradres { get; set; }
    }
}
