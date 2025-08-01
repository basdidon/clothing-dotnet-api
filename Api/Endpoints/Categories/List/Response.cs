namespace Api.Endpoints.Categories.List
{
    public class Response
    {
        public CategoryDto[] Categories { get; set; } = [];
        public int TotalCount { get; set; }
    }
}
