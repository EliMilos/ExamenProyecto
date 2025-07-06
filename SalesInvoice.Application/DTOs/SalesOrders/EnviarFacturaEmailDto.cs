using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesInvoice.Application.DTOs.SalesOrders
{
    public class EnviarFacturaEmailDto
    {
        [FromForm]
        public IFormFile Pdf { get; set; }

        [FromForm]
        public string Email { get; set; }
    }
}
