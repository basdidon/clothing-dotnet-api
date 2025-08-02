using Api.Constants;
using Api.Models;
using Api.Persistance;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Api.Endpoints.Orders.Create
{
    public class Request
    {
        public List<OrderLineDto> OrderLines { get; set; } = [];
    }

    public class OrderLineDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class Response
    {
        public Guid OrderId { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class Endpoint(ApplicationDbContext context, PaymentIntentService service) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var productIds = req.OrderLines.Select(x => x.ProductId);

            var productDict = await context.Products.Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, ct);

            var quantityDict = req.OrderLines.ToDictionary(x => x.ProductId, x => x.Quantity);

            Models.Order order = new()
            {
                CreatedAt = DateTime.UtcNow,
            };

            foreach (var productId in productIds)
            {
                var product = productDict[productId];
                order.OrderLines.Add(new OrderLine()
                {
                    Product = product,
                    ProductName = productDict[productId].Title,
                    UnitPrice = productDict[productId].UnitPrice,
                    Quantity = quantityDict[productId]
                });
            }
            // store Order
            await context.Orders.AddAsync(order, ct);
            await context.SaveChangesAsync(ct);

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

            OrderPayment stripeOrderPayment = new()
            {
                Order = order,
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret,
                CreatedAt = DateTime.UtcNow,
            };

            await context.OrderPayments.AddAsync(stripeOrderPayment, ct);
            await context.SaveChangesAsync(ct);

            Response = new()
            {
                OrderId = order.Id,
                PaymentIntentId = paymentIntent.Id,
                ClientSecret = paymentIntent.ClientSecret
            };
        }
    }
}
