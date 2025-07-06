using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesInvoice.Application.DTOs.SalesOrders;
using SalesInvoice.Application.UseCases.SalesOrders.Commands;
using SalesInvoice.Application.UseCases.SalesOrders.Queries;
using System.Net.Mail;
using System.Net;

namespace SalesInvoice.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<IActionResult> CreateSalesOrder([FromBody] CreateSalesOrderDto dto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var command = new RegisterSalesOrderCommand(dto);
            var salesOrderId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = salesOrderId }, new { Id = salesOrderId });
        }

        [HttpPost("EnviarCorreo")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EnviarCorreo([FromForm] EnviarFacturaEmailDto dto)
        {
            if (dto.Pdf == null || string.IsNullOrEmpty(dto.Email))
                return BadRequest("Faltan datos");

            using var ms = new MemoryStream();
            await dto.Pdf.CopyToAsync(ms);
            ms.Position = 0;

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sebastian27guerrero@gmail.com", "eoit lnzi odiz rxqz"),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress("sebastian27guerrero@gmail.com"),
                Subject = "Factura electrónica",
                Body = "Adjunto encontrará su factura en PDF.",
                IsBodyHtml = false
            };

            message.To.Add(dto.Email);
            message.Attachments.Add(new Attachment(ms, dto.Pdf.FileName, "application/pdf"));

            try
            {
                await smtp.SendMailAsync(message);
                return Ok(new { message = "Correo enviado" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al enviar correo: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
        {
            var query = new GetSalesOrderByIdQuery(id);
            var salesOrder = await _mediator.Send(query, cancellationToken);

            if (salesOrder == null)
                return NotFound();

            return Ok(salesOrder);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Usuario")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var query = new GetAllSalesOrdersQuery();
            var orders = await _mediator.Send(query, cancellationToken);
            return Ok(orders);
        }

        // Controller
        [HttpGet("last")]
        public async Task<IActionResult> GetLastOrderNumber()
        {
            var lastNumber = await _mediator.Send(new GetLastSalesOrderNumberQuery());
            return Ok(new { numeroFactura = lastNumber });
        }


        // Controller
        [HttpGet("date")]
        public async Task<IActionResult> GetDateServerAsync()
        {
            var lastNumber = await _mediator.Send(new GetDateServerQuery());
            return Ok(new { fechaFactura = lastNumber });
        }

    }
}

