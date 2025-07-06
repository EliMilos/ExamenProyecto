using MediatR;
using SalesInvoice.Domain.Entities;
using SalesInvoice.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.SalesOrders.Queries
{
    public class GetLastSalesOrderNumberQuery : IRequest<int?> { }

    public class GetLastSalesOrderNumberHandler : IRequestHandler<GetLastSalesOrderNumberQuery, int?>
    {
        private readonly ISalesOrderRepository _repository;

        public GetLastSalesOrderNumberHandler(ISalesOrderRepository repository)
        {
            _repository = repository;
        }

        public async Task<int?> Handle(GetLastSalesOrderNumberQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetSalesOrderCountAsync();
        }
    }


}