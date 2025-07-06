using MediatR;
using SalesInvoice.Application.DTOs.SalesOrders;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries;

public class GetSalesOrderByIdQueryHandler : IRequestHandler<GetSalesOrderByIdQuery, SalesOrderDto?>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetSalesOrderByIdQueryHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<SalesOrderDto?> Handle(GetSalesOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
        if (order == null)
            return null;

        return new SalesOrderDto
        {
            SalesOrderId = order.SalesOrderId,
            OrderDate = order.Date,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.FirstName + " " + order.Customer?.LastName,
            TotalAmount = order.Total,
            OrderDetails = order.OrderDetails.Select(d => new OrderDetailDto
            {
                ProductId = d.ProductId,
                ProductName = d.Product?.Name,
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity
            }).ToList()
        };
    }

}
