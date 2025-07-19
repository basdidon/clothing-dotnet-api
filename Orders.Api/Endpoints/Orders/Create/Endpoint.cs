using Contract;
using FastEndpoints;
using MassTransit;

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

    public class Endpoint(IRequestClient<GetProductsRequest> client) : Endpoint<Request, decimal>
    {
        public override void Configure()
        {
            Post("/orders");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var productIds = req.OrderLines.Select(x => x.ProductId).ToList();
            var response = await client.GetResponse<GetProductsResponse>(new GetProductsRequest(productIds), ct);

            // calculate total price
            //var totalPrice = response.Message.Products.Sum(x => x.UnitPrice);
            //Response = totalPrice;
            // apply discounts if any
            
            // save order to database
            // retunn stripe client secret for payment
        }
    }
}