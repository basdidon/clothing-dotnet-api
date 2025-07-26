using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Products.Api.Models;
using Products.Api.Persistance;

namespace Products.Api.Features.Products.GetById
{
    public class Request
    {
        public Guid ProductId { get; set; }
    }

    public class Endpoint(ApplicationDbContext context) : Endpoint<Request, Product>
    {
        public override void Configure()
        {
            Get("/products/{productId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == req.ProductId, cancellationToken: ct);
            if (product == null)
            {
                await SendNotFoundAsync(ct);
            }
            else
            {
                await SendOkAsync(product, ct);
            }

        }
    }
}
