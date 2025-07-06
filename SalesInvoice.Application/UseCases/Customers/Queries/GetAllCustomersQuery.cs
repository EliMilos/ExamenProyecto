using MediatR;
using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Application.UseCases.Customers.Queries
{
    public class GetAllCustomersQuery : IRequest<IEnumerable<Customer>>
    {
    }
}
