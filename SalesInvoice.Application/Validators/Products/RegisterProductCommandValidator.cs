using FluentValidation;
using SalesInvoice.Application.UseCases.Products.Commands;

namespace SalesInvoice.Application.Validators.Products
{
    public class RegisterProductCommandValidator : AbstractValidator<RegisterProductCommand>
    {
        public RegisterProductCommandValidator()
        {
            RuleFor(x => x.ProductDto).NotNull().WithMessage("El producto es obligatorio.");

            When(x => x.ProductDto != null, () =>
            {
                RuleFor(x => x.ProductDto.Code)
                    .NotEmpty().WithMessage("El código del producto es obligatorio.")
                    .MaximumLength(20).WithMessage("El código no puede superar los 20 caracteres.");

                RuleFor(x => x.ProductDto.Name)
                    .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                    .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

                RuleFor(x => x.ProductDto.Category)
                    .NotEmpty().WithMessage("La categoría es obligatoria.")
                    .MaximumLength(100).WithMessage("La categoría no puede superar los 100 caracteres.");

                RuleFor(x => x.ProductDto.UnitPrice)
                    .GreaterThan(0).WithMessage("El precio unitario debe ser mayor que cero.");

                RuleFor(x => x.ProductDto.Stock)
                    .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
            });
        }
    }
}
