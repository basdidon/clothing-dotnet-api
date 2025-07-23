using Contract;
using Contract.Payments;
using MassTransit;
using Payment.Api.Models;
using Stripe.Checkout;

namespace Payment.Api.Consumers
{
    public class CheckoutConsumer(SessionService service, IRequestClient<GetProductsRequest> getProductsClient) : IConsumer<CheckoutRequest>
    {
        public async Task Consume(ConsumeContext<CheckoutRequest> context)
        {
            var productIds = context.Message.CheckoutItems.Select(x => x.Value.ProductId).ToList();
            var response = await getProductsClient.GetResponse<GetProductsResponse>(new GetProductsRequest(productIds));

            var products = response.Message.Products;

            var domain = "http://localhost:3000";
            var options = new SessionCreateOptions
            {
                UiMode = "custom",
                LineItems =
                [
                    ..products.Select(p => new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            UnitAmount = (long)(p.Value.UnitPrice * 100), // Convert dollars to cents
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = p.Value.Title,
                            },
                        },
                        Quantity = context.Message.CheckoutItems[p.Key].Quantity,
                    }),
                ],
                Mode = "payment",
                PaymentMethodTypes = ["card"],
                ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
            };

            Session session = service.Create(options);

            await context.RespondAsync(new CheckoutResponse(session.ClientSecret));
        }
    }
}
