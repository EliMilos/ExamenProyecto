using MediatR;
using SalesInvoice.Application.DTOs.Customers;

namespace SalesInvoice.Application.UseCases.Customers.Commands
{
    public class RegisterCustomerCommand : IRequest<int>  // devuelve el Id creado
    {
        public CreateCustomerDto CustomerDto { get; set; }

        public RegisterCustomerCommand(CreateCustomerDto dto)
        {
            CustomerDto = dto;
        }
    }
}
