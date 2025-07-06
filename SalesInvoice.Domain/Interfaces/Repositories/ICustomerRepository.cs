using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Domain.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int customerId);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<int> AddAsync(Customer customer, CancellationToken cancellationToken);
    Task<bool> UpdateAsync(Customer customer, byte[] rowVersion);
    Task DeleteAsync(int customerId);
}
