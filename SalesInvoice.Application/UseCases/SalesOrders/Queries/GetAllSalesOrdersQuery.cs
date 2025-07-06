using MediatR;
using SalesInvoice.Application.DTOs.SalesOrders;
using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries
{
    public class GetAllSalesOrdersQuery : IRequest<IEnumerable<SalesOrderDto>>
    {
    }
}
