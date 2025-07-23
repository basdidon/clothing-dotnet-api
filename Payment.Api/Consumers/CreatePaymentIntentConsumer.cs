using Contract.Payments;
using MassTransit;
using Payment.Api.Models;
using Payment.Api.Persistence;
using Stripe;

namespace Payment.Api.Consumers
{
    public class CreatePaymentIntentConsumer(PaymentIntentService service, ApplicationDbContext dbContext) : IConsumer<CreatePaymentIntentRequest>
    {
        public async Task Consume(ConsumeContext<CreatePaymentIntentRequest> context)
        {
            //var domain = "http://localhost:3000";
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(context.Message.TotalAmount) * 100, // Convert dollars to cents
                Currency = "usd",
                PaymentMethodTypes = ["card"],
                Metadata = new Dictionary<string, string>
                {
                    { "order_id", context.Message.OrderId.ToString() }
                },
            };
               // ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",

            var paymentIntent = service.Create(options);

            OrderPayment orderPayment = new()
            {
                OrderId = context.Message.OrderId,
                Provider = "Stripe",
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                CreatedAt = DateTime.UtcNow,
            };

            await dbContext.AddAsync(orderPayment);
            await dbContext.SaveChangesAsync();

            await context.RespondAsync(new CreatePaymentIntentResponse(paymentIntent.ClientSecret));
        }
    }
}
