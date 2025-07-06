using MediatR;
using SalesInvoice.Application.DTOs.SalesOrders;

namespace SalesInvoice.Application.UseCases.SalesOrders.Commands
{
    public class RegisterSalesOrderCommand : IRequest<string>
    {
        public CreateSalesOrderDto SalesOrderDto { get; }

        public RegisterSalesOrderCommand(CreateSalesOrderDto dto)
        {
            SalesOrderDto = dto;
        }
    }
}
