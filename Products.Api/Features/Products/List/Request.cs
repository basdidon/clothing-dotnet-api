namespace Products.Api.Features.Products.List
{
    public class Request
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 24;

        public int? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
    }
}
