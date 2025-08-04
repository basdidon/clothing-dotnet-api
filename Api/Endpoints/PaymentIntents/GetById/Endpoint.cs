using Api.DTOs;
using Api.Persistance;
using FastEndpoints;
using Stripe;
using Microsoft.EntityFrameworkCore;
using Api.Constants;

namespace Api.Endpoints.PaymentIntents.GetById
{
    public class Request
    {
        public string PaymentIntentId { get; set; } = string.Empty;
    }

    public class Endpoint(PaymentIntentService service, ApplicationDbContext context) : Endpoint<Request, OrderPaymentIntentDto>
    {
        public override void Configure()
        {
            Get("payment-intents/{PaymentIntentId}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var orderPaymentIntent = await context.OrderPaymentIntents
                .FirstOrDefaultAsync(x => x.PaymentIntentId == req.PaymentIntentId, ct);

            if (orderPaymentIntent == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            // fetch payment intent from stripe
            var paymentIntent = await service.GetAsync(req.PaymentIntentId, cancellationToken: ct);

            // update fetch data to our table
            orderPaymentIntent.Status = PaymentIntentStatusExtensions.FromStripeString(paymentIntent.Status);
            await context.SaveChangesAsync(ct);

            // return updated value
            Response = OrderPaymentIntentDto.Map(orderPaymentIntent);
        }
    }
}
