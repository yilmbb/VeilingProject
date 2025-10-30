namespace VeilingAPI.DTOs
{
    /// <summary>
    /// Data Transfer Object voor gebruiker responses
    /// Bevat alle gebruiker info + het type (Koper/Verkoper)
    /// </summary>
    public class GebruikerResponseDto
    {
        public int gebruiker_id { get; set; }
        public string email { get; set; } = string.Empty;
        public string wachtwoord { get; set; } = string.Empty;
        public DateTime registreren { get; set; }
        public DateTime? inloggen { get; set; }
        public string username { get; set; } = string.Empty;

        // Type indicator: "Koper" of "Verkoper"
        public string Type { get; set; } = string.Empty;

        // Bedrijfsgegevens (voor zowel Koper als Verkoper)
        public string? bedrijfsnaam { get; set; }
        public string? kvk { get; set; }
        public string? btw_nummer { get; set; }
        public string? telefoon { get; set; }
        public string? rekening_nummer { get; set; }

        // Verkoper-specifiek
        public string? bedrijfsadres { get; set; }

        // Koper-specifiek
        public string? leveradres { get; set; }
    }
}
