using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Orders.List
{
    public class Endpoint(ApplicationDbContext context) : EndpointWithoutRequest<OrderDto[]>
    {
        public override void Configure()
        {
            Get("/orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var orders = await context.Orders.AsNoTracking()
                .Include(x => x.OrderLines)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

            Response = [.. orders.Select(x => OrderDto.Map(x))];
        }
    }
}
