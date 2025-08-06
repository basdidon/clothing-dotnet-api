using Api.DTOs;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Products.List
{
    public class Endpoint(ApplicationDbContext context) : Endpoint<Request, PageResponse<ProductDto>>
    {
        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var queryable = context.Products.Include(x => x.Thumbnail).AsNoTracking();

            // filter by categoryId
            if (req.CategoryId.HasValue)
            {
                queryable = queryable.Where(x => x.CategoryId == req.CategoryId);
            }

            // filter by categoryName
            if (!string.IsNullOrWhiteSpace(req.CategoryName))
            {
                queryable = queryable.Where(x => x.Category != null && x.Category.Name == req.CategoryName);
            }

            Response = await queryable.ToPageResponseAsync(req.Page, req.PageSize, ProductDto.Map, ct);
        }
    }
}
