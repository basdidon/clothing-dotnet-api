using FastEndpoints;
using MassTransit;
using Orders.Api.Models;
using Orders.Api.Persistance;

namespace Orders.Api.Endpoints.Orders.Create
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
        public string ClientSecret { get; set; } = string.Empty;
    }

    public class Endpoint(IRequestClient<GetProductsRequest> getProductsClient, IRequestClient<CreatePaymentIntentRequest> createPaymentIntentClient, ApplicationDbContext context) : Endpoint<Request, Response>
    {
        public override void Configure()
        {
            Post("/orders/checkout");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var productIds = req.OrderLines.Select(x => x.ProductId).ToList();
            var response = await getProductsClient
                .GetResponse<GetProductsResponse>(new GetProductsRequest(productIds));

            var products = response.Message.Products;

            Models.Order order = new()
            {
                OrderLines = [.. req.OrderLines.Select((x) => new OrderLine()
                {
                    ProductId = x.ProductId, 
                    ProductName = products[x.ProductId].Title, 
                    Quantity = x.Quantity, 
                    UnitPrice = products[x.ProductId].UnitPrice 
                })]
            };

            await context.AddAsync(order, ct);

            // checkout
            var createPaymentIntentResponse = await createPaymentIntentClient
                .GetResponse<CreatePaymentIntentResponse>(new CreatePaymentIntentRequest(order.Id, order.TotalAmount), ct);

            // save order to database
            await context.SaveChangesAsync(ct);

            // retunn stripe client secret for payment
            await SendOkAsync(new Response { ClientSecret = createPaymentIntentResponse.Message.ClientSecret }, ct);
        }
    }
}