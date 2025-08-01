using Api.Models;

namespace Api.Endpoints.Products.List
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
