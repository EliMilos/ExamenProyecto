using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.UseCases.Customers.Commands
{
    public class DeleteCustomerCommand : IRequest<bool>  // aquí Unit también
    {
        public int CustomerId { get; }

        public DeleteCustomerCommand(int customerId)
        {
            CustomerId = customerId;
        }
    }
}
