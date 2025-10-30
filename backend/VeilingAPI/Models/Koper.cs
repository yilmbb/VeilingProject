using System.ComponentModel.DataAnnotations;

namespace VeilingAPI.Models
{
    public class Koper : Gebruiker
    {
        [Required]
        [StringLength(200)]
        public string bedrijfsnaam { get; set; } = string.Empty;

        [Required]
        [StringLength(8)]
        public string kvk { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string btw_nummer { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string telefoon { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string leveradres { get; set; } = string.Empty;

        [Required]
        [StringLength(34)]
        public string rekening_nummer { get; set; } = string.Empty;
    }
}
