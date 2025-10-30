namespace VeilingAPI.DTOs
{
    public class GebruikerResponseDto
    {
        public int gebruiker_id { get; set; }
        public string email { get; set; } = string.Empty;
        public string wachtwoord { get; set; } = string.Empty;
        public DateTime registreren { get; set; }
        public DateTime? inloggen { get; set; }
        public string username { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string? bedrijfsnaam { get; set; }
        public string? kvk { get; set; }
        public string? btw_nummer { get; set; }
        public string? telefoon { get; set; }
        public string? rekening_nummer { get; set; }
        public string? bedrijfsadres { get; set; }
        public string? leveradres { get; set; }
    }
}
