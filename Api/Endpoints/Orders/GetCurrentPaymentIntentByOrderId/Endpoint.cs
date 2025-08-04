using Api.Constants;
using Api.DTOs;
using Api.Endpoints.Orders.GetValidPaymentIntentByOrderId;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Api.Endpoints.Orders.GetCurrentPaymentIntentByOrderId
{
    public class Endpoint(ApplicationDbContext context, PaymentIntentService service) : Endpoint<Request, OrderPaymentIntentDto>
    {
        public override void Configure()
        {
            Get("orders/{OrderId}/payment-intents/current");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var orderPaymentIntent = await context.OrderPaymentIntents
                .Where(x => x.OrderId == req.OrderId)
                .SingleOrDefaultAsync(x => x.IsCurrent, ct);

            if (orderPaymentIntent == null)
            {
                AddError(x => x.OrderId, $"OrderPaymentIntent with orderId : {req.OrderId} was not found.");
                await Send.ErrorsAsync(404, ct);
                return;
            }

            var paymentIntent = await service.GetAsync(orderPaymentIntent.PaymentIntentId, cancellationToken: ct);

            // update OrderPaymentIntent
            orderPaymentIntent.Status = PaymentIntentStatusExtensions.FromStripeString(paymentIntent.Status);
            await context.SaveChangesAsync(ct);

            if (orderPaymentIntent.Status == PaymentIntentStatus.Canceled)
            {
                AddError("The current payment intent is no longer valid. Please create a new one.");
                await Send.ErrorsAsync(409, ct);
            }

            Response = OrderPaymentIntentDto.Map(orderPaymentIntent);
        }
    }
}
