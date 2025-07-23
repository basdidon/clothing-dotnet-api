using FastEndpoints;
using Stripe;

namespace Payment.Api.Endpoints.Checkouts.GetStatus
{
    public class Request
    {
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
            Get("/checkout/{PaymentIntentId}/status");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            PaymentIntent session = service.Get(req.PaymentIntentId);
            

            await SendOkAsync(new Response
            {
                Status = session.Status,
                CustomerEmail = session.ReceiptEmail ?? string.Empty
            }, ct);
        }
    }
}
