using MediatR;
using SalesInvoice.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class UpdateProductCommand : IRequest<bool>
    {
        public UpdateProductDto ProductDto { get; }

        public UpdateProductCommand(UpdateProductDto productDto)
        {
            ProductDto = productDto;
        }
    }
}
