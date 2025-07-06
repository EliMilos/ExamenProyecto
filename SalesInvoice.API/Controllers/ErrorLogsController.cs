using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.UseCases.ErrorLogs.Queries;

namespace SalesInvoice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ErrorLogsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ErrorLogsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtener todos los registros de errores (solo Admin).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        var logs = await _mediator.Send(new GetAllErrorLogsQuery());
        return Ok(logs);
    }

    /// <summary>
    /// Obtener el detalle de un error por Id (solo Admin).
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var log = await _mediator.Send(new GetErrorLogByIdQuery(id));
        return log != null ? Ok(log) : NotFound();
    }
}
