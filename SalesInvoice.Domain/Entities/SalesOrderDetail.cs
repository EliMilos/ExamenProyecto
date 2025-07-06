
namespace SalesInvoice.Domain.Entities;

public class SalesOrderDetail
{
    public int SalesOrderDetailId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Tax { get; set; }
    public decimal Subtotal { get; set; }

    public string SalesOrderId { get; set; } = default!;
    public SalesOrder SalesOrder { get; set; } = default!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = default!;
}

