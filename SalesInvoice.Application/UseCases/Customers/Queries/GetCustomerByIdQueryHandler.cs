using MediatR;
using SalesInvoice.Application.DTOs.Customers;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Customers.Queries;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ReadCustomerDto?>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ReadCustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
        if (customer == null)
            return null;

        return new ReadCustomerDto
        {
            CustomerId = customer.CustomerId,
            Cedula = customer.Cedula,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            Address = customer.Address,
            Phone = customer.Phone,
            RowVersion = customer.RowVersion != null ? Convert.ToBase64String(customer.RowVersion) : null
        };
    }
}
