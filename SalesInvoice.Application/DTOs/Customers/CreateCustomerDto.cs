namespace SalesInvoice.Application.DTOs.Customers;

public class CreateCustomerDto
{
    public string Cedula { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
}



