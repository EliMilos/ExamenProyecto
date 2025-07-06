using MediatR;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);
            if (product == null)
            {
                return false;
            }

            await _productRepository.DeleteAsync(request.ProductId);
            return true;
        }
    }
}
