namespace Api.Models
{
    public class ImageObject
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

        public Guid? ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
