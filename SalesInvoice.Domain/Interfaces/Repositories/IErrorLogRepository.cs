using SalesInvoice.Domain.Entities;

namespace SalesInvoice.Domain.Interfaces.Repositories;

public interface IErrorLogRepository
{
    /// <summary>
    /// Obtiene todos los registros de errores.
    /// </summary>
    Task<IEnumerable<ErrorLog>> GetAllAsync();

    /// <summary>
    /// Obtiene un registro de error por su identificador.
    /// </summary>
    Task<ErrorLog?> GetByIdAsync(int errorLogId);

    /// <summary>
    /// Inserta un nuevo registro de error.
    /// </summary>
    Task AddAsync(ErrorLog log);
}
