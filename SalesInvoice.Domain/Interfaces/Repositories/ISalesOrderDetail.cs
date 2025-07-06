using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Domain.Interfaces.Repositories;

public interface ISalesOrderDetailRepository
{
    Task<SalesOrderDetail?> GetByIdAsync(int salesOrderDetailId);
    Task<IEnumerable<SalesOrderDetail>> GetAllAsync();
    Task<int> AddAsync(SalesOrderDetail salesOrderDetail, CancellationToken cancellationToken);
    Task UpdateAsync(SalesOrderDetail salesOrderDetail);
    Task DeleteAsync(int salesOrderDetailId);
}
