using System.ComponentModel.DataAnnotations;

namespace VeilingAPI.DTOs
{
    /// <summary>
    /// Request model voor het updaten van een gebruiker
    /// </summary>
    public class UpdateGebruikerRequest
    {
        [Required]
        public int gebruiker_id { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string wachtwoord { get; set; } = string.Empty;

        // Bedrijfsgegevens (voor zowel Koper als Verkoper)
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

        // Adressen (afhankelijk van type gebruiker)
        public string? bedrijfsadres { get; set; }
        public string? leveradres { get; set; }
    }
}
