using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Products.Api.Models;
using Products.Api.Persistance;

namespace Products.Api.Features.Products.List
{
    public class ListProductsEndpoint(ApplicationDbContext context) : EndpointWithoutRequest<Product[]>
    {

        public override void Configure()
        {
            Get("/products");
            AllowAnonymous();
        }

        public override async Task HandleAsync( CancellationToken ct)
        {
            var products = await context.Products.AsNoTracking().ToListAsync(ct);
            Response = [..products];
        }
    }
}
