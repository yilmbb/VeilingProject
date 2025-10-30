using System.ComponentModel.DataAnnotations;

namespace VeilingAPI.DTOs
{
    public class RegistratieRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string wachtwoord { get; set; } = string.Empty;

        [Required]
        public string Type { get; set; } = string.Empty;

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

        [Required]
        public string adres { get; set; } = string.Empty;
    }
}
