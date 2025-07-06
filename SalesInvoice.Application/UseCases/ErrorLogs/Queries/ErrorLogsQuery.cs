using MediatR;
using SalesInvoice.Application.DTOs.ErrorLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.ErrorLogs.Queries;

public record GetAllErrorLogsQuery() : IRequest<IEnumerable<ErrorLogDto>>;

public record GetErrorLogByIdQuery(int ErrorLogId) : IRequest<ErrorLogDto?>;
