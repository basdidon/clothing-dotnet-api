using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("cors:allowUrls").Get<string[]>()!);
        });
});
// key : "Stripe:Apikey" should be set in appsettings.json or environment variables
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Apikey").Value!;

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);

app.MapGet("/", () => "Hello World!"); 
app.MapPost("/create-checkout-session", () =>
{
    var domain = "http://localhost:3000";
    var options = new SessionCreateOptions
    {
        UiMode = "custom",
        LineItems =
        [
            new SessionLineItemOptions
            {
                // Provide the exact Price ID (for example, price_1234) of the product you want to sell
                //Price = "{{PRICE_ID}}",
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "usd",
                    UnitAmount = 5000, // $50.00 (in cents)
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "Custom T-Shirt"
                    }
                },
                Quantity = 1,
            },
        ],
        Mode = "payment",
        PaymentMethodTypes = ["card"],
        ReturnUrl = domain + "/return?session_id={CHECKOUT_SESSION_ID}",
    };
    var service = new SessionService();
    Session session = service.Create(options);

    return Results.Json(new { clientSecret = session.ClientSecret });
});

app.MapGet("/session-status", ([FromQuery] string session_id) =>
{
    var sessionService = new SessionService();
    Session session = sessionService.Get(session_id);

    return Results.Json(new { status = session.Status, customer_email = session.CustomerDetails.Email });
});
app.Run();
