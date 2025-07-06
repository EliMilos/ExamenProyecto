using MediatR;
using SalesInvoice.Application.DTOs.Products;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class RegisterProductCommand : IRequest<int>  // Devuelve el ProductId creado
    {
        public CreateProductDto ProductDto { get; set; }

        public RegisterProductCommand(CreateProductDto dto)
        {
            ProductDto = dto;
        }
    }
}
