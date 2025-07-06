using FluentValidation;
using SalesInvoice.Application.DTOs.Auth;

namespace SalesInvoice.Application.Validators.Auth
{
    public class RegisterWithRoleDtoValidator : AbstractValidator<RegisterWithRoleDto>
    {
        public RegisterWithRoleDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo es obligatorio.")
                .EmailAddress().WithMessage("El correo no tiene un formato válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
                .MaximumLength(10).WithMessage("La contraseña no debe exceder los 10 caracteres.")
                .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
                .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
                .Matches(@"\d").WithMessage("La contraseña debe contener al menos un dígito.")
                .Matches(@"[\W_]").WithMessage("La contraseña debe contener al menos un carácter especial.");

            RuleFor(x => x.Cedula)
                .NotEmpty().WithMessage("La cédula es obligatoria.")
                .Length(10).WithMessage("La cédula debe tener 10 dígitos.")
                .Matches(@"^\d{10}$").WithMessage("La cédula debe contener solo números.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("El rol es obligatorio.");
        }
    }
}
