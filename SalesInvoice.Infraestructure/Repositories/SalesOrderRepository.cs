using Microsoft.EntityFrameworkCore;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.DataContexts;
using System.Data;

namespace SalesInvoice.Infrastructure.Repositories;

public class SalesOrderRepository : ISalesOrderRepository
{
    private readonly SalesInvoiceDbContext _context;

    public SalesOrderRepository(SalesInvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<string> AddAsync(SalesOrder salesOrder, CancellationToken cancellationToken)
    {
        await _context.SalesOrders.AddAsync(salesOrder, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return salesOrder.SalesOrderId;
    }

    public async Task<SalesOrder?> GetByIdAsync(string salesOrderId)
    {
        return await _context.SalesOrders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product) // Incluye el producto de cada detalle
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(o => o.SalesOrderId == salesOrderId);
    }


    public async Task<IEnumerable<SalesOrder>> GetAllAsync()
    {
        return await _context.SalesOrders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product) // Incluye el producto asociado a cada detalle
            .Include(o => o.Customer)
            .ToListAsync();
    }

    public async Task<DateTime> GetDateServerAsync()
    {
        using var connection = _context.Database.GetDbConnection();
        using var command = connection.CreateCommand();

        command.CommandText = "SELECT GETDATE()";
        command.CommandType = CommandType.Text;

        if (connection.State != ConnectionState.Open)
            await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        return Convert.ToDateTime(result);
    }


    public async Task UpdateAsync(SalesOrder salesOrder)
    {
        _context.SalesOrders.Update(salesOrder);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string salesOrderId)
    {
        var order = await _context.SalesOrders
            .Include(o => o.OrderDetails)
            .FirstOrDefaultAsync(o => o.SalesOrderId == salesOrderId);

        if (order != null)
        {
            _context.SalesOrderDetails.RemoveRange(order.OrderDetails);
            _context.SalesOrders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int?> GetSalesOrderCountAsync()
    {
        return await _context.SalesOrders.CountAsync();
    }

    public Task<IEnumerable<SalesOrder>> GetSalesOrdersByCustomerIdAsync(string customerId)
    {
        throw new NotImplementedException();
    }

    
}
