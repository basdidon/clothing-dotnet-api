using Api.Models;
using Api.Utilities;

namespace Api.Endpoints.Products
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }

        public string[] AvalilableSizes { get; set; } = [];

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } 

        public string? ThumbnailUrl { get; set; }
        public string[] ImageUrls { get; set; } = [];

        public static ProductDto Map(Product product)
        => new()
        {
            Id = product.Id,
            Title = product.Title,
            SubTitle = product.SubTitle,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            AvalilableSizes = AvailableSizeHelper.GetAvaliableSizesAsStrings(product.AvaliableSizes),
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name,
            ThumbnailUrl = product.Thumbnail?.ImagePath,
            ImageUrls = [..product.ImageUrls.Select(x=>x.ImagePath)]
        };
    }
}
