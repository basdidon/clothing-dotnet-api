using Api.DTOs;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Orders.ListPaymentIntent
{
    public class Request
    {
        public Guid OrderId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class Endpoint(ApplicationDbContext context) : Endpoint<Request, PageResponse<OrderPaymentIntentDto>>
    {
        public override void Configure()
        {
            Get("orders/{OrderId}/payment-intents");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var queryable = context.OrderPaymentIntents.AsNoTracking()
                 .Where(x => x.OrderId == req.OrderId);

            Response = await queryable.ToPageResponseAsync(req.Page, req.PageSize, OrderPaymentIntentDto.Map, ct);
        }
    }
}