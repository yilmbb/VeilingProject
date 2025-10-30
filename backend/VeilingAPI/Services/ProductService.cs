using VeilingAPI.Models;
using VeilingAPI.Repositories;

namespace VeilingAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> MaakProductAanAsync(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (string.IsNullOrWhiteSpace(product.naam))
            {
                throw new ArgumentException("Product naam is verplicht", nameof(product.naam));
            }

            if (product.verkoper_id <= 0)
            {
                throw new ArgumentException("Ongeldige verkoper", nameof(product.verkoper_id));
            }

            product.aangemaakt_op = DateTime.UtcNow;
            return await _productRepository.AddAsync(product);
        }

        public async Task<IEnumerable<Product>> GetProductenVoorVerkoperAsync(int verkoperId)
        {
            if (verkoperId <= 0)
            {
                throw new ArgumentException("Ongeldige verkoper", nameof(verkoperId));
            }

            return await _productRepository.GetByVerkoperIdAsync(verkoperId);
        }
    }
}


