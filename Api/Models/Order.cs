namespace Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public List<OrderLine> OrderLines { get; set; } = [];

        public decimal TotalAmount => OrderLines.Sum(x => x.TotalPrice);

        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }
}
