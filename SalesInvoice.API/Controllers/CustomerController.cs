using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.DTOs.Customers;
using SalesInvoice.Application.Exceptions;
using SalesInvoice.Application.UseCases.Customers.Commands;
using SalesInvoice.Application.UseCases.Customers.Queries;

namespace SalesInvoice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Solo Admin puede crear clientes
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] CreateCustomerDto dto)
    {
        var command = new RegisterCustomerCommand(dto);
        var newCustomerId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = newCustomerId }, newCustomerId);
    }

    // Tanto Admin como Usuario pueden obtener un cliente por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Usuario")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery(id));
        return customer != null ? Ok(customer) : NotFound();
    }

    // Tanto Admin como Usuario pueden obtener todos los clientes
    [HttpGet]
    [Authorize(Roles = "Admin,Usuario")]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _mediator.Send(new GetAllCustomersQuery());
        return Ok(customers);
    }

    // Solo Admin puede actualizar clientes
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        dto.CustomerId = id;

        try
        {
            var result = await _mediator.Send(new UpdateCustomerCommand(dto));
            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (ConcurrencyException ex)
        {
            // Aquí devuelves 409 y el mensaje para que el front sepa que hay conflicto
            return Conflict(new { message = ex.Message });
        }
    }


    // Solo Admin puede eliminar clientes
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteCustomerCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
