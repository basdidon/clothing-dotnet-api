using Api.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public decimal UnitPrice { get; set; }

        public AvaliableSizes AvaliableSizes { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; } = null!;

        public ImageObject? Thumbnail { get; set; }
        public Guid? ThumbnailId { get; set; }

        public ICollection<ImageObject> Images { get; set; } = [];
    }

    public class ProductTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title)
                .HasMaxLength(50);
            builder.Property(x => x.SubTitle)
                .HasMaxLength(120);
            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.HasOne(p => p.Thumbnail)
                   .WithOne()
                   .HasForeignKey<Product>(p => p.ThumbnailId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId);
        }
    }
}
