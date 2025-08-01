using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Models
{
    public class OrderPayment
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public OrderPaymentStatus Status { get; set; } = OrderPaymentStatus.Pending;

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }

    public class OrderPaymentTypeConfiguration : IEntityTypeConfiguration<OrderPayment>
    {
        public void Configure(EntityTypeBuilder<OrderPayment> builder)
        {
            builder.HasKey(x => x.PaymentIntentId);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.PaymentIntentId);
            builder.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("now()");
        }
    }
}