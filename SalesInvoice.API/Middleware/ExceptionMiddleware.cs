using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Se capturó un error no controlado.");

            var routeData = context.GetRouteData();
            var controller = routeData?.Values["controller"]?.ToString() ?? "Unknown";
            var action = routeData?.Values["action"]?.ToString() ?? "Unknown";
            var userEmail = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";

            // Guardar el error en la base de datos
            using (var scope = _serviceProvider.CreateScope())
            {
                var errorLogRepository = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();

                var errorLog = new ErrorLog
                {
                    Message = ex.Message,
                    Detail = ex.ToString(),
                    Controller = controller,
                    Method = action,
                    UserEmail = userEmail,
                    LoggedAt = DateTime.UtcNow
                };

                await errorLogRepository.AddAsync(errorLog);
            }

            // Definir código de estado según el tipo de excepción
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex is ApplicationException ? 400 : 500;

            var response = new { error = ex.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}