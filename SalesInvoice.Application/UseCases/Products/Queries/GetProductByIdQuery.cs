using MediatR;
using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Application.UseCases.Products.Queries
{
    public class GetProductByIdQuery : IRequest<Product?>
    {
        public int ProductId { get; }

        public GetProductByIdQuery(int productId)
        {
            ProductId = productId;
        }
    }
}
