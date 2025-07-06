using MediatR;
using SalesInvoice.Application.DTOs.SalesOrders;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries;

public class GetAllSalesOrdersQueryHandler : IRequestHandler<GetAllSalesOrdersQuery, IEnumerable<SalesOrderDto>>
{
    private readonly ISalesOrderRepository _salesOrderRepository;

    public GetAllSalesOrdersQueryHandler(ISalesOrderRepository salesOrderRepository)
    {
        _salesOrderRepository = salesOrderRepository;
    }

    public async Task<IEnumerable<SalesOrderDto>> Handle(GetAllSalesOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _salesOrderRepository.GetAllAsync();

        return orders.Select(o => new SalesOrderDto
        {
            SalesOrderId = o.SalesOrderId,
            OrderDate = o.Date,
            CustomerId = o.CustomerId,
            CustomerName = o.Customer?.FirstName + " " + o.Customer?.LastName,
            OrderDetails = o.OrderDetails.Select(d => new OrderDetailDto
            {
                ProductId = d.ProductId,
                ProductName = d.Product?.Name, // Aquí ya viene el ProductName porque lo incluiste en el Include
                UnitPrice = d.UnitPrice,
                Quantity = d.Quantity
            }).ToList(),
            TotalAmount = o.Total
        });
    }

}
