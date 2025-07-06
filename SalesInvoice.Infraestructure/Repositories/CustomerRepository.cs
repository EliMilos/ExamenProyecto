using Microsoft.EntityFrameworkCore;
using SalesInvoice.Application.Exceptions;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.DataContexts;
using System.Data;

namespace SalesInvoice.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly SalesInvoiceDbContext _context;

        public CustomerRepository(SalesInvoiceDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<int> AddAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customer.CustomerId;
        }

        public async Task<bool> UpdateAsync(Customer customer, byte[] originalRowVersion)
        {
            _context.Entry(customer).Property(c => c.RowVersion).OriginalValue = originalRowVersion;
            _context.Customers.Update(customer);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConcurrencyException("El registro fue modificado por otro usuario. Por favor, actualice la página.");
            }
        }



        public async Task DeleteAsync(int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
