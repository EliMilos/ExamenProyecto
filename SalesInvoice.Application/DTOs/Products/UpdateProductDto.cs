namespace SalesInvoice.Application.DTOs.Products
{
    public class UpdateProductDto
    {
        public int ProductId { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Category { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public string RowVersion { get; set; } = string.Empty;
    }
}
