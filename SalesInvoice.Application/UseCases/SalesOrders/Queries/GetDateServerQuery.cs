using MediatR;
using SalesInvoice.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries
{
    public class GetDateServerQuery : IRequest<DateTime>
    {
        public class GetDateServerQueryHandler : IRequestHandler<GetDateServerQuery, DateTime>
        {
            private readonly ISalesOrderRepository _repository;

            public GetDateServerQueryHandler(ISalesOrderRepository repository)
            {
                _repository = repository;
            }

            public async Task<DateTime> Handle(GetDateServerQuery request, CancellationToken cancellationToken)
            {
                return await _repository.GetDateServerAsync();
            }
        }
    }
}
