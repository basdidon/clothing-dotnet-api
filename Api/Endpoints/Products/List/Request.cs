using System.ComponentModel;

namespace Api.Endpoints.Products.List
{
    public class Request
    {
        [DefaultValue(1)]
        public int Page { get; set; }
        [DefaultValue(24)]
        public int PageSize { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
    }
}
