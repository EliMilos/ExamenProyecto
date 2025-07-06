using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Domain.Interfaces.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int productId);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<int> AddAsync(Product product, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Product product, byte[] rowVersion);
    Task DeleteAsync(int productId);
    Task<IEnumerable<Product>> GetAllWithStockAsync();
}
