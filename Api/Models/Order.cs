using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public List<OrderLine> OrderLines { get; set; } = [];

        public decimal TotalAmount => OrderLines.Sum(x => x.TotalPrice);

        public ICollection<OrderPaymentIntent> OrderPaymentIntents { get; set; } = [];

        public bool IsPaid { get; set; } = false;

        public DateTime CreatedAt { get; set; }
    }

    public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.OrderLines)
                .WithOne(x => x.Order)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
