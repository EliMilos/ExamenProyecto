using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.DTOs.Products
{
    public class CreateProductDto
    {
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
    }
}
