using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Products.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int ProductId { get; }

        public DeleteProductCommand(int productId)
        {
            ProductId = productId;
        }
    }
}
