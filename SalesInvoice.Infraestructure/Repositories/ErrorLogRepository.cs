using Microsoft.EntityFrameworkCore;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using SalesInvoice.Infrastructure.DataContexts;

public class ErrorLogRepository : IErrorLogRepository
{
    private readonly SalesInvoiceDbContext _context;

    public ErrorLogRepository(SalesInvoiceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ErrorLog>> GetAllAsync()
    {
        return await _context.ErrorLogs.OrderByDescending(e => e.LoggedAt).ToListAsync();
    }

    public async Task<ErrorLog?> GetByIdAsync(int id)
    {
        return await _context.ErrorLogs.FindAsync(id);
    }

    public async Task AddAsync(ErrorLog log)
    {
        await _context.ErrorLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }
}
