using Microsoft.EntityFrameworkCore;
using SalesInvoice.Application.Exceptions;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.DataContexts;

namespace SalesInvoice.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SalesInvoiceDbContext _context;

        public ProductRepository(SalesInvoiceDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllWithStockAsync()
        {
            return await _context.Products
                .Where(p => p.Stock > 0)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product.ProductId;
        }

        public async Task<bool> UpdateAsync(Product product, byte[] originalRowVersion)
        {
            _context.Entry(product).Property(p => p.RowVersion).OriginalValue = originalRowVersion;
            _context.Products.Update(product);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
