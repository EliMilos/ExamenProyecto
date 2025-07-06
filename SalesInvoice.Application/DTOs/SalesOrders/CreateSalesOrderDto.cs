namespace SalesInvoice.Application.DTOs.SalesOrders;

public class CreateSalesOrderDto
{
    public int CustomerId { get; set; }
    public List<CreateSalesOrderDetailDto> OrderDetails { get; set; } = new();
}

public class CreateSalesOrderDetailDto
{
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
}
