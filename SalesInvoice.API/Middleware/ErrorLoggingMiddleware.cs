using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.API.Middleware;

public class ErrorLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorLoggingMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ErrorLoggingMiddleware(RequestDelegate next, ILogger<ErrorLoggingMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught by middleware");

            var routeData = context.GetRouteData();
            var controller = routeData?.Values["controller"]?.ToString() ?? "Unknown";
            var action = routeData?.Values["action"]?.ToString() ?? "Unknown";
            var userEmail = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity.Name : "Anonymous";

            var errorLog = new ErrorLog
            {
                Message = ex.Message,
                Detail = ex.ToString(),
                Controller = controller,
                Method = action,
                UserEmail = userEmail,
                LoggedAt = DateTime.UtcNow
            };

            // Crear un scope para resolver servicios scoped
            using (var scope = _serviceProvider.CreateScope())
            {
                var errorLogRepository = scope.ServiceProvider.GetRequiredService<IErrorLogRepository>();
                await errorLogRepository.AddAsync(errorLog);
            }

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Internal Server Error",
                message = ex.Message
            });
        }
    }
}
