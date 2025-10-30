using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VeilingAPI.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int product_id { get; set; }

        [Required]
        [StringLength(200)]
        public string naam { get; set; } = string.Empty;

        [StringLength(2000)]
        public string? beschrijving { get; set; }

        [Range(0, double.MaxValue)]
        public decimal prijs { get; set; }

        [Range(0, int.MaxValue)]
        public int voorraad { get; set; }

        [Required]
        public DateTime aangemaakt_op { get; set; } = DateTime.UtcNow;

        // Relatie naar Verkoper
        [Required]
        public int verkoper_id { get; set; }

        [ForeignKey(nameof(verkoper_id))]
        public Verkoper? Verkoper { get; set; }
    }
}


