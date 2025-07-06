
namespace SalesInvoice.Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }
    public string Cedula { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public byte[]? RowVersion { get; set; }

    public ICollection<SalesOrder> SalesOrders { get; set; } = new List<SalesOrder>();
}
