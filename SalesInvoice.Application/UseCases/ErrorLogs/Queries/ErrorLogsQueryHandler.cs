using MediatR;
using SalesInvoice.Application.DTOs.ErrorLogs;
using SalesInvoice.Domain.Interfaces.Repositories;

namespace SalesInvoice.Application.UseCases.ErrorLogs.Queries;

public class GetAllErrorLogsQueryHandler : IRequestHandler<GetAllErrorLogsQuery, IEnumerable<ErrorLogDto>>
{
    private readonly IErrorLogRepository _repository;

    public GetAllErrorLogsQueryHandler(IErrorLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ErrorLogDto>> Handle(GetAllErrorLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repository.GetAllAsync();

        return logs.Select(log => new ErrorLogDto
        {
            ErrorLogId = log.ErrorLogId,
            Message = log.Message,
            Detail = log.Detail,
            Controller = log.Controller,
            Method = log.Method,
            UserEmail = log.UserEmail,
            LoggedAt = log.LoggedAt
        });
    }
}

public class GetErrorLogByIdQueryHandler : IRequestHandler<GetErrorLogByIdQuery, ErrorLogDto?>
{
    private readonly IErrorLogRepository _repository;

    public GetErrorLogByIdQueryHandler(IErrorLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorLogDto?> Handle(GetErrorLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await _repository.GetByIdAsync(request.ErrorLogId);
        if (log == null)
            return null;

        return new ErrorLogDto
        {
            ErrorLogId = log.ErrorLogId,
            Message = log.Message,
            Detail = log.Detail,
            Controller = log.Controller,
            Method = log.Method,
            UserEmail = log.UserEmail,
            LoggedAt = log.LoggedAt
        };
    }
}
