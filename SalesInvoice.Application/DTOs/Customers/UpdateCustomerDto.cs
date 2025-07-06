namespace SalesInvoice.Application.DTOs.Customers;

public class UpdateCustomerDto
{
    public int CustomerId { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string RowVersion { get; set; } = string.Empty;  // Hexadecimal como string
}
