using Api.Constants;

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

        public ImageUrl? Thumbnail;
        public Guid? ThumbnailId { get; set; }

        public ImageUrl[] ImageUrls { get; set; } = [];
    }
}
