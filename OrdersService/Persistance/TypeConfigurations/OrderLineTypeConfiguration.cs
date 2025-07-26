using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Api.Models;

namespace Orders.Api.Persistance.TypeConfigurations
{
    public class OrderLineTypeConfiguration : IEntityTypeConfiguration<OrderLine>
    {
        public void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });
            builder.Property(x => x.ProductName)
                .HasMaxLength(50);
        }
    }
}
