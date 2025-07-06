
namespace SalesInvoice.Domain.Entities;

public class Product
{
    public int ProductId { get; set; }
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Category { get; set; }
    public decimal UnitPrice { get; set; }
    public int Stock { get; set; }

    public byte[]? RowVersion { get; set; } // Concurrency token

    public ICollection<SalesOrderDetail> SalesOrderDetails { get; set; } = new List<SalesOrderDetail>();
}
