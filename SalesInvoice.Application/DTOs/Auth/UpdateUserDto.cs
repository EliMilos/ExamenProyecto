namespace SalesInvoice.Application.DTOs.Auth
{
    public class UpdateUserDto
    {
        public string Email { get; set; } = default!;
        public string Cedula { get; set; } = default!;
        public string ConcurrencyStamp { get; set; } = default!;
        public string Role { get; set; } = default!;

        public string NewPassword { get; set; } = default!;


    }
}
