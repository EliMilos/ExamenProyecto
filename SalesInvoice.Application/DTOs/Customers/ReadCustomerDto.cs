using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.DTOs.Customers
{
    public class ReadCustomerDto
    {
        public int CustomerId { get; set; }
        public string Cedula { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? RowVersion { get; set; }  // base64 string
    }
}
