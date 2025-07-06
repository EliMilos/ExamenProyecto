using FluentValidation;
using SalesInvoice.Application.DTOs.Auth;

namespace SalesInvoice.Application.Validators.Auth;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo es obligatorio.")
            .EmailAddress().WithMessage("El correo no tiene un formato válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.");
    }
}
