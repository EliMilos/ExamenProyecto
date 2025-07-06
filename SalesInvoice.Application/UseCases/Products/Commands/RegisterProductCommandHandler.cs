using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class RegisterProductCommandHandler : IRequestHandler<RegisterProductCommand, int>
    {
        private readonly IProductRepository _productRepository;

        public RegisterProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(RegisterProductCommand request, CancellationToken cancellationToken)
        {
            var dto = request.ProductDto;

            var newProduct = new Product
            {
                Code = dto.Code,
                Name = dto.Name,
                Category = dto.Category,
                UnitPrice = dto.UnitPrice,
                Stock = dto.Stock
            };

            return await _productRepository.AddAsync(newProduct, cancellationToken);
        }
    }
}
