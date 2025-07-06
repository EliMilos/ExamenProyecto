using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Products.Queries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product?>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetByIdAsync(request.ProductId);
        }
    }
}
