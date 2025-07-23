namespace Orders.Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }
       
        public List<OrderLine> OrderLines { get; set; } = [];
       
        public decimal TotalAmount => OrderLines.Sum(x => x.TotalPrice);
        
        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }

    public class OrderLine
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
