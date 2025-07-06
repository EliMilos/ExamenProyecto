using Microsoft.AspNetCore.Identity;

namespace SalesInvoice.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Cedula { get; set; } = string.Empty;
        // Otras propiedades personalizadas
    }
}
