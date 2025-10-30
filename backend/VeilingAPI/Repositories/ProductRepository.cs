using Microsoft.EntityFrameworkCore;
using VeilingAPI.Data;
using VeilingAPI.Models;

namespace VeilingAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly VeilingDbContext _context;

        public ProductRepository(VeilingDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product product)
        {
            await _context.Producten.AddAsync(product);
            await SaveChangesAsync();
            return product;
        }

        public async Task<IEnumerable<Product>> GetByVerkoperIdAsync(int verkoperId)
        {
            return await _context.Producten
                .Where(p => p.verkoper_id == verkoperId)
                .OrderByDescending(p => p.aangemaakt_op)
                .ToListAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}


