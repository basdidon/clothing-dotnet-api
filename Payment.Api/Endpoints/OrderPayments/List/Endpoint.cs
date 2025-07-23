using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Payment.Api.Models;
using Payment.Api.Persistence;

namespace Payment.Api.Endpoints.OrderPayments.List
{
    public class Endpoint(ApplicationDbContext context) : EndpointWithoutRequest<OrderPayment[]>
    {
        public override void Configure()
        {
            Get("/order-payments");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var orderPayments= await context.OrderPayments.AsNoTracking()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);
                //.ForEach(x => x.ClientSecret = string.Empty); // Hide sensitive information

            Response = [.. orderPayments];
        }
    }
}
