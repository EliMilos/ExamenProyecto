using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SalesInvoice.Application.Behaviors;
using SalesInvoice.Application.Services;
using System.Reflection;
using FluentValidation;

namespace SalesInvoice.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registra MediatR y sus handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Registra todos los validators de FluentValidation en esta capa
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Registra el Pipeline de validación
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // Otros servicios de Application
            services.AddScoped<AuthService>();

            return services;
        }
    }
}
