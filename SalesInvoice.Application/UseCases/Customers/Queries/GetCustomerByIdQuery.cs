using MediatR;
using SalesInvoice.Application.DTOs.Customers;

namespace SalesInvoice.Application.UseCases.Customers.Queries;

public class GetCustomerByIdQuery : IRequest<ReadCustomerDto?>
{
    public int CustomerId { get; }

    public GetCustomerByIdQuery(int customerId)
    {
        CustomerId = customerId;
    }
}
