using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Domain.Interfaces.Repositories;

public interface ISalesOrderRepository
{
    Task<SalesOrder?> GetByIdAsync(string salesOrderId);
    Task<IEnumerable<SalesOrder>> GetAllAsync();
    Task<string> AddAsync(SalesOrder salesOrder, CancellationToken cancellationToken);
    Task UpdateAsync(SalesOrder salesOrder);
    Task DeleteAsync(string salesOrderId);
    Task<int?> GetSalesOrderCountAsync();
    Task<IEnumerable<SalesOrder>> GetSalesOrdersByCustomerIdAsync(string customerId);
    Task<DateTime> GetDateServerAsync();
}
