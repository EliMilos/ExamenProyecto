using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Customers.Commands
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand, int>
    {
        private readonly ICustomerRepository _customerRepository;

        public RegisterCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CustomerDto;

            var newCustomer = new Customer
            {
                Cedula = dto.Cedula,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Address = dto.Address,
                Phone = dto.Phone
            };

            return await _customerRepository.AddAsync(newCustomer, cancellationToken);
        }
    }

}
