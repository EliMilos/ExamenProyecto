using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Customers.Queries
{
    public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersQueryHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _customerRepository.GetAllAsync();
        }
    }
}
