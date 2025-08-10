using System.ComponentModel;

namespace Api.Endpoints.Categories.List
{
    public class Request
    {
        [DefaultValue(1)]
        public int Page { get; set; }
        [DefaultValue(24)]
        public int PageSize { get; set; }
    }
}
