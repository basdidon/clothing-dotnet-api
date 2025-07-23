using Products.Api.Models;

namespace Products.Api.Features.Products.List
{
    public class Response
    {
        public Product[] Data { get; set; } = [];
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
    }
}
