using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Categories.List
{
    public class Endpoint(ApplicationDbContext context):EndpointWithoutRequest<Response>
    {
        public override void Configure()
        {
            Get("categories");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var categories =  await context.Categories.AsNoTracking().ToListAsync(ct);
            Response = new()
            {
                Categories = [..categories.Select(x => CategoryDto.Map(x))],
                TotalCount = categories.Count
            };
        }
    }
}
