using SentryExample.Core.Models;

namespace SentryExample.Core.Services
{
    public interface IProductService
    {
        Task<Product> GetByIdAsync(int id);
        Task InsertProductAsync(ProductDto productDto);
    }
}
