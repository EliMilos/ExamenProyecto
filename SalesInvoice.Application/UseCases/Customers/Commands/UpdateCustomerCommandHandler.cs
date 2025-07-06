using MediatR;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.Customers.Commands;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;

    public UpdateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CustomerDto;

        var existingCustomer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (existingCustomer == null)
            return false;

        // Convierte Base64 a byte[]
        var rowVersionBytes = Convert.FromBase64String(dto.RowVersion);

        existingCustomer.Cedula = dto.Cedula;
        existingCustomer.FirstName = dto.FirstName;
        existingCustomer.LastName = dto.LastName;
        existingCustomer.Email = dto.Email;
        existingCustomer.Address = dto.Address;
        existingCustomer.Phone = dto.Phone;

        // Envía los bytes para la comparación
        return await _customerRepository.UpdateAsync(existingCustomer, rowVersionBytes);
    }
}
