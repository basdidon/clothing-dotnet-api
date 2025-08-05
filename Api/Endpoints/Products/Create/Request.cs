namespace Api.Endpoints.Products.Create
{
    public class Request
    {
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string[] AvailableSizes { get; set; } = [];

        public decimal UnitPrice { get; set; }

        public IFormFile? Thumbnail { get; set; } = null!;
        //public IFormFileCollection? Images { get; set; }
    }
}