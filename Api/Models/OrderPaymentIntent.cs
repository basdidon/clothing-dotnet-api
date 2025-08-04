using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Models
{
    public class OrderPaymentIntent
    {
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public PaymentIntentStatus Status { get; set; } = PaymentIntentStatus.RequiresPaymentMethod; 

        public DateTime CreatedAt { get; set; }

        public bool IsCurrent { get; set; } = true;
    }

    public class OrderPaymentTypeConfiguration : IEntityTypeConfiguration<OrderPaymentIntent>
    {
        public void Configure(EntityTypeBuilder<OrderPaymentIntent> builder)
        {
            builder.HasKey(x => x.PaymentIntentId);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.PaymentIntentId);
            builder.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("now()");

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderPaymentIntents)
                .HasForeignKey(x => x.OrderId);
        }
    }
}