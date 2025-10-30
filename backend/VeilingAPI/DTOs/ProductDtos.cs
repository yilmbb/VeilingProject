using System.ComponentModel.DataAnnotations;

namespace VeilingAPI.DTOs
{
    public class ProductCreateRequest
    {
        [Required]
        public string naam { get; set; } = string.Empty;
        public string? beschrijving { get; set; }
        [Range(0, double.MaxValue)]
        public decimal prijs { get; set; }
        [Range(0, int.MaxValue)]
        public int voorraad { get; set; }
        [Required]
        [EmailAddress]
        public string verkoperEmail { get; set; } = string.Empty;
    }
    public class ProductResponse
    {
        public int product_id { get; set; }
        public string naam { get; set; } = string.Empty;
        public string? beschrijving { get; set; }
        public decimal prijs { get; set; }
        public int voorraad { get; set; }
        public DateTime aangemaakt_op { get; set; }
    }
}


