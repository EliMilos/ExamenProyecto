using MediatR;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ProductDto;
            var existingProduct = await _productRepository.GetByIdAsync(dto.ProductId);
            if (existingProduct == null)
            {
                return false;
            }

            // Convierte Base64 a byte[]
            var rowVersionBytes = Convert.FromBase64String(dto.RowVersion);

            existingProduct.Code = dto.Code;
            existingProduct.Name = dto.Name;
            existingProduct.Category = dto.Category;
            existingProduct.UnitPrice = dto.UnitPrice;
            existingProduct.Stock = dto.Stock;

            await _productRepository.UpdateAsync(existingProduct, rowVersionBytes);
            return true;
        }
    }
}
