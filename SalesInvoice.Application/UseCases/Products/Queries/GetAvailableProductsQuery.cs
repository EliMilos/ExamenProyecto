using MediatR;
using SalesInvoice.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Products.Queries
{
    public class GetAvailableProductsQuery : IRequest<IEnumerable<Product>>
    {
    }
}
