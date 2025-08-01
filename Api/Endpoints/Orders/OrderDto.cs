using Api.Models;

namespace Api.Endpoints.Orders
{
    public class OrderDto
    {
        public Guid Id { get; set; }

        public List<OrderLineDto> OrderLines { get; set; } = [];

        public decimal TotalAmount => OrderLines.Sum(x => x.TotalPrice);

        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public static OrderDto Map(Order order) => new()
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            IsPaid = order.IsPaid,
            OrderLines = [.. order.OrderLines.Select(x => new OrderLineDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice
            })]
        };
    }

    public class OrderLineDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
