using FluentValidation;
using SalesInvoice.Application.UseCases.Customers.Commands;

namespace SalesInvoice.Application.Validators.Customers;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.CustomerDto).NotNull().WithMessage("El cliente es obligatorio.");

        When(x => x.CustomerDto != null, () =>
        {
            RuleFor(x => x.CustomerDto.CustomerId)
                .GreaterThan(0).WithMessage("El ID del cliente debe ser válido.");

            RuleFor(x => x.CustomerDto.Cedula)
                .NotEmpty().WithMessage("La cédula es obligatoria.")
                .Length(10).WithMessage("La cédula debe tener 10 dígitos.");

            RuleFor(x => x.CustomerDto.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

            RuleFor(x => x.CustomerDto.LastName)
                .NotEmpty().WithMessage("El apellido es obligatorio.")
                .MaximumLength(100).WithMessage("El apellido no puede superar los 100 caracteres.");

            RuleFor(x => x.CustomerDto.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");

            RuleFor(x => x.CustomerDto.Address)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede superar los 200 caracteres.");

            RuleFor(x => x.CustomerDto.Phone)
                .NotEmpty().WithMessage("El teléfono es obligatorio.")
                .Matches(@"^\d{7,10}$").WithMessage("El teléfono debe tener entre 7 y 10 dígitos.");
        });
    }
}
