using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.DTOs.Products;
using SalesInvoice.Application.UseCases.Products.Commands;
using SalesInvoice.Application.UseCases.Products.Queries;

namespace SalesInvoice.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Solo Admin puede crear productos
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register([FromBody] CreateProductDto dto)
    {
        var command = new RegisterProductCommand(dto);
        var newProductId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = newProductId }, newProductId);
    }

    // Tanto Admin como Usuario pueden obtener un producto por id
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Usuario")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product != null ? Ok(product) : NotFound();
    }

    // Tanto Admin como Usuario pueden obtener todos los productos
    [HttpGet]
    [Authorize(Roles = "Admin,Usuario")]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    [HttpGet("available")]
    [Authorize(Roles = "Admin,Usuario")]
    public async Task<IActionResult> GetAvailableForInvoice()
    {
        var products = await _mediator.Send(new GetAvailableProductsQuery());
        return Ok(products);
    }

    // Solo Admin puede actualizar productos
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        dto.ProductId = id;
        var result = await _mediator.Send(new UpdateProductCommand(dto));
        return result ? NoContent() : NotFound();
    }

    // Solo Admin puede eliminar productos
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteProductCommand(id));
        return deleted ? NoContent() : NotFound();
    }
}
