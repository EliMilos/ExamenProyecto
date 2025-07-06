using MediatR;
using SalesInvoice.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Customers.Commands
{
    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
            {
                return false;
            }

            await _customerRepository.DeleteAsync(request.CustomerId);
            return true;
        }
    }

}
