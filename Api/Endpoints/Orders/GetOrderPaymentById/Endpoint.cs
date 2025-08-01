using FastEndpoints;
using Stripe;

namespace Api.Endpoints.Orders.GetOrderPaymentById
{
    public class Request
    {
        public Guid OrderId { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
    }

    public class Response
    {
        public string Status { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }

    public class Endpoint(PaymentIntentService service) : Endpoint<Request>
    {
        public override void Configure()
        {
            Get("/paymentIntents/{PaymentIntentId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            PaymentIntent session = service.Get(req.PaymentIntentId);

            await Send.OkAsync(new Response
            {
                Status = session.Status,
                CustomerEmail = session.ReceiptEmail ?? string.Empty
            }, ct);
        }
    }
}
