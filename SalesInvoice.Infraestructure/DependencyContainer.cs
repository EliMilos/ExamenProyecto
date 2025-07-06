using Microsoft.Extensions.DependencyInjection;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.Options;
using SalesInvoice.Infrastructure.Repositories;

namespace SalesInvoice.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, Action<DBOptions> configureDBOptions)
        {

            services.Configure(configureDBOptions);

            // Registrar Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISalesOrderRepository, SalesOrderRepository>();
            services.AddScoped<IErrorLogRepository, ErrorLogRepository>();


            return services;
        }
    }
}
