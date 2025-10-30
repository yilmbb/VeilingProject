using VeilingAPI.Models;

namespace VeilingAPI.Services
{
    public interface IProductService
    {
        Task<Product> MaakProductAanAsync(Product product);
        Task<IEnumerable<Product>> GetProductenVoorVerkoperAsync(int verkoperId);
    }
}


