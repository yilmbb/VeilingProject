using VeilingAPI.Models;

namespace VeilingAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Product> AddAsync(Product product);
        Task<IEnumerable<Product>> GetByVerkoperIdAsync(int verkoperId);
        Task<int> SaveChangesAsync();
    }
}


