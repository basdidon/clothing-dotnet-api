using Api.Models;

namespace Api.Endpoints.Categories.List
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public static CategoryDto Map(Category category) => new()
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
