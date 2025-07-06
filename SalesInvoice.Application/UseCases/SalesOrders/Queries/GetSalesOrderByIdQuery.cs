using MediatR;
using SalesInvoice.Application.DTOs.SalesOrders;
using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries;

public class GetSalesOrderByIdQuery : IRequest<SalesOrderDto?>
{
    public string SalesOrderId { get; }

    public GetSalesOrderByIdQuery(string salesOrderId)
    {
        SalesOrderId = salesOrderId;
    }
}