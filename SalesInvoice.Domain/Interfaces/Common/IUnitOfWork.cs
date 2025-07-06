
namespace SalesInvoice.Domain.Interfaces.Common;

public interface IUnitOfWork
{
    Task<int> CommitAsync();
}
