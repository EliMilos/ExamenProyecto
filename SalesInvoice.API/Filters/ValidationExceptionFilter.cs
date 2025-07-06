using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SalesInvoice.API.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationException validationException)
            {
                var errors = validationException.Errors.Select(e => new
                {
                    PropertyName = e.PropertyName,
                    ErrorMessage = e.ErrorMessage
                });

                context.Result = new BadRequestObjectResult(new { Errors = errors });
                context.ExceptionHandled = true;
            }
        }
    }
}
