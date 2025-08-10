using Api.DTOs;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Categories.List
{
    public class Endpoint(ApplicationDbContext context):Endpoint<Request,PageResponse<CategoryDto>>
    {
        public override void Configure()
        {
            Get("categories");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req,CancellationToken ct)
        {
            Response =  await context.Categories.AsNoTracking()
                .ToPageResponseAsync(req.Page,req.PageSize,CategoryDto.Map,ct);
        }
    }
}
