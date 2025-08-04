using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.PaymentIntents.List
{
    public class Request
    {
        public Guid? OrderId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class Endpoint(ApplicationDbContext context) : Endpoint<Request>
    {
        public override void Configure()
        {
            Get("payment-intents");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var queryable = context.OrderPaymentIntents.AsNoTracking();

            if (Guid.Empty != req.OrderId)
            {
                queryable = queryable.Where(x => x.OrderId == req.OrderId);
            }

            await queryable.Skip(req.PageSize * (req.Page - 1))
                .Take(req.PageSize)
                .ToListAsync(ct);
        }
    }
}
