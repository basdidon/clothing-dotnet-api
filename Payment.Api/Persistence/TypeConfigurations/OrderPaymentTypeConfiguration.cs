using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payment.Api.Models;

namespace Payment.Api.Persistence.TypeConfigurations
{
    public class OrderPaymentTypeConfiguration : IEntityTypeConfiguration<OrderPayment>
    {
        public void Configure(EntityTypeBuilder<OrderPayment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.PaymentIntentId);
            builder.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("now()");
        }
    }
}
