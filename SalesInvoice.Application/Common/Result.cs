
namespace SalesInvoice.Application.Common;

public class Result
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Result Ok(string message = "Operation succeeded") => new Result { Success = true, Message = message };
    public static Result Fail(string message = "Operation failed") => new Result { Success = false, Message = message };
}
