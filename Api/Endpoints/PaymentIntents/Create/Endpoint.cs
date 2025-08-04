using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Api.Endpoints.PaymentIntents.Create
{
    public class Endpoint(PaymentIntentService service, ApplicationDbContext context) : Endpoint<Request>
    {
        public override void Configure()
        {
            Post("payment-intents");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var order = await context.Orders
                .Include(x => x.OrderPaymentIntents)
                .FirstOrDefaultAsync(x => x.Id == req.OrderId, ct);

            if (order == null)
            {
                await Send.NotFoundAsync(ct);
                return;
            }

            await context.OrderPaymentIntents.Where(x => x.OrderId == req.OrderId)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(x => x.IsCurrent, false)
                    , ct);

            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(order.TotalAmount) * 100, // Convert dollars to cents
                Currency = "usd",
                PaymentMethodTypes = ["card"],
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", order.Id.ToString() }
                },
            };

            var paymentIntent = service.Create(options);

            Models.OrderPaymentIntent stripeOrderPayment = new()
            {
                Order = order,
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                CreatedAt = DateTime.UtcNow,
            };

            await context.OrderPaymentIntents.AddAsync(stripeOrderPayment, ct);
            await context.SaveChangesAsync(ct);
        }
    }
}
