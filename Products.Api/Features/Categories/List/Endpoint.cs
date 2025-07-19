using FastEndpoints;

namespace Products.Api.Features.Categories.List
{
    public class Endpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("/categories");
            AllowAnonymous();
        }

        public override Task HandleAsync(CancellationToken ct)
        {
            return base.HandleAsync(ct);
        }
    }
}
