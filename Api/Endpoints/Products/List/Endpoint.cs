using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Products.List
{
    public class Endpoint(ApplicationDbContext context) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var queryable = context.Products.AsNoTracking();

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

            // apply pagination
            int totalItems = await context.Products.CountAsync(cancellationToken: ct);
            int totalPages = (int)Math.Ceiling(totalItems / (double)req.PageSize);
            queryable = queryable.Skip((req.Page - 1) * req.PageSize).Take(req.PageSize);

            // execute the query
            var products = await queryable.ToListAsync(ct);

            // map to response
            Response = new Response()
            {
                Data = [.. products.Select(x=>ProductDto.Map(x))],
                CurrentPage = req.Page,
                PageSize = req.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };

        }
    }
}
