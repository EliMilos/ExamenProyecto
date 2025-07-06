
namespace SalesInvoice.Domain.Entities;

public class ErrorLog
{
    public int ErrorLogId { get; set; }
    public string Message { get; set; } = default!;
    public string? Detail { get; set; }
    public string? Controller { get; set; }
    public string? Method { get; set; }
    public string? UserEmail { get; set; }
    public DateTime LoggedAt { get; set; }
}


