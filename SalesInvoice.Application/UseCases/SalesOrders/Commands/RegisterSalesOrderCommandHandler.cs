using MediatR;
using Microsoft.EntityFrameworkCore;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.DataContexts;

namespace SalesInvoice.Application.UseCases.SalesOrders.Commands;

public class RegisterSalesOrderCommandHandler : IRequestHandler<RegisterSalesOrderCommand, string>
{
    private readonly SalesInvoiceDbContext _context;
    private readonly ISalesOrderRepository _repository;
    private readonly IProductRepository _productRepository;

    public RegisterSalesOrderCommandHandler(SalesInvoiceDbContext context, ISalesOrderRepository repository, IProductRepository productRepository)
    {
        _context = context;
        _repository = repository;
        _productRepository = productRepository;
    }

    public async Task<string> Handle(RegisterSalesOrderCommand request, CancellationToken cancellationToken)
    {
        var dto = request.SalesOrderDto;
        var salesOrderId = Guid.NewGuid().ToString("N");

        // Abrimos transacción
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            // Validar stock de todos los productos antes de continuar
            var updatedProducts = new List<Product>();

            foreach (var detail in dto.OrderDetails)
            {
                var product = await _productRepository.GetByIdAsync(detail.ProductId);
                if (product == null)
                    throw new ApplicationException($"Producto con ID {detail.ProductId} no existe.");

                if (product.Stock < detail.Quantity)
                    throw new ApplicationException($"Stock insuficiente para el producto {product.Name}. Disponible: {product.Stock}, solicitado: {detail.Quantity}");

                product.Stock -= detail.Quantity;
                updatedProducts.Add(product);
            }

            // Crear detalles de la orden
            var orderDetails = dto.OrderDetails.Select(d => new SalesOrderDetail
            {
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Subtotal = d.UnitPrice * d.Quantity,
                Tax = d.UnitPrice * d.Quantity * 0.12m
            }).ToList();

            // Crear la orden
            var salesOrder = new SalesOrder
            {
                SalesOrderId = salesOrderId,
                CustomerId = dto.CustomerId,
                Date = DateTime.Now,
                OrderDetails = orderDetails,
                Subtotal = orderDetails.Sum(x => x.Subtotal),
                Tax = orderDetails.Sum(x => x.Tax),
                Total = orderDetails.Sum(x => x.Subtotal + x.Tax)
            };

            // Registrar la orden
            await _repository.AddAsync(salesOrder, cancellationToken);

            // Actualizar stock de los productos
            foreach (var product in updatedProducts)
            {
                await _productRepository.UpdateAsync(product, product.RowVersion);
            }

            // Confirmar la transacción
            await transaction.CommitAsync(cancellationToken);

            return salesOrderId;
        }
        catch (DbUpdateConcurrencyException)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw new ApplicationException("El registro fue modificado por otro usuario. Por favor, actualice la página.");
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
