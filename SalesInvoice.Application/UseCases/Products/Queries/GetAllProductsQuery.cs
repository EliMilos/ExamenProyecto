using MediatR;
using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Application.UseCases.Products.Queries
{
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
