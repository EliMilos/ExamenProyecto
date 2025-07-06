namespace SalesInvoice.Application.DTOs.SalesOrders
{
    public class SalesOrderDto
    {
        public string SalesOrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
