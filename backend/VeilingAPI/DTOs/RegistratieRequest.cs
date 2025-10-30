using System.ComponentModel.DataAnnotations;

namespace VeilingAPI.DTOs
{
    /// <summary>
    /// Request model voor gebruiker registratie
    /// Bevat Type indicator om te weten of het een Koper of Verkoper is
    /// </summary>
    public class RegistratieRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string wachtwoord { get; set; } = string.Empty;

        // Type indicator: "Koper" of "Verkoper"
        [Required]
        public string Type { get; set; } = string.Empty;

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

        // Adres (kan bedrijfsadres of leveradres zijn, afhankelijk van Type)
        [Required]
        public string adres { get; set; } = string.Empty;
    }
}
