
namespace SalesInvoice.Domain.Entities;

public class SalesOrder
{
    public string SalesOrderId { get; set; } = default!;
    public DateTime Date { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }

    public byte[]? RowVersion { get; set; } // Concurrency token

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = default!;

    public ICollection<SalesOrderDetail> OrderDetails { get; set; } = new List<SalesOrderDetail>();
}

