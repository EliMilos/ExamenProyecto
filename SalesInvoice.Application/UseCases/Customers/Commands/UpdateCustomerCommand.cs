using MediatR;
using SalesInvoice.Application.DTOs.Customers;

namespace SalesInvoice.Application.UseCases.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<bool>
    {
        public UpdateCustomerDto CustomerDto { get; }

        public UpdateCustomerCommand(UpdateCustomerDto customerDto)
        {
            CustomerDto = customerDto;
        }
    }
}
