using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Products.GetById
{
    public class Request
    {
        public Guid ProductId { get; set; }
    }

    public class Endpoint(ApplicationDbContext context) : Endpoint<Request, ProductDto>
    {
        public override void Configure()
        {
            Get("/products/{productId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var product = await context.Products.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == req.ProductId, cancellationToken: ct);
            
            if (product == null)
            {
                await Send.NotFoundAsync(ct);
            }
            else
            {
                await Send.OkAsync(ProductDto.Map(product), ct);
            }
        }
    }
}
