using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Products.Queries;

public class GetAvailableProductsQueryHandler : IRequestHandler<GetAvailableProductsQuery, IEnumerable<Product>>
{
    private readonly IProductRepository _productRepository;

    public GetAvailableProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> Handle(GetAvailableProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllWithStockAsync();
    }
}
