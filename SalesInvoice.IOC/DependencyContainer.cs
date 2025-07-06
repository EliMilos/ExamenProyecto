using Microsoft.Extensions.DependencyInjection;
using SalesInvoice.Infrastructure;
using SalesInvoice.Infrastructure.Options;
using SalesInvoice.Application;

namespace SalesInvoice.IOC
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddSalesInvoiceServices(this IServiceCollection services, Action<DBOptions> configureDBOptions)
        {
            services.AddInfrastructure(configureDBOptions)
                .AddApplication();

            return services;
        }
    }
}
