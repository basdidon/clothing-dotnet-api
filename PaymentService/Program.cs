using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payment.Api.Consumers;
using Payment.Api.Persistence;
using SharedLibrary.FastEndpoint.Filters;
using SharedLibrary.Masstransit;
using SharedLibrary.Persistance.Extensions;
using SharedLibrary.Settings;
using Stripe;
using Stripe.Checkout;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// key : "Stripe:Apikey" should be set in appsettings.json or environment variables
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Apikey").Value!;

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<PaymentIntentService>();

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CheckoutConsumer>();
    x.AddConsumer<CreatePaymentIntentConsumer>();

    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, configurator) =>
    {
        configurator.ConnectConsumeObserver(new LoggingConsumeObserver());
        configurator.UseDelayedMessageScheduler();
        var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;
        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(builder.Configuration.GetSection("cors:allowUrls").Get<string[]>()!);
        });
});


builder.Services
   .AddFastEndpoints(o => o.IncludeAbstractValidators = true)
   .SwaggerDocument(o =>
   {
       o.MaxEndpointVersion = 1;
       o.DocumentSettings = s =>
       {
           s.DocumentName = "Initial Release";
           s.Title = "Payment API";
           s.Version = "v1";
       };
   });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.EnsureDatabaseCreatedAsync<ApplicationDbContext>(resetOnStart:true);
}

app.UseCors(MyAllowSpecificOrigins);

app.UseFastEndpoints(c =>
{
    c.Endpoints.Configurator = ep =>
    {
        ep.Options(b => b.AddEndpointFilter<EndpointRequestFilter>());
    };
    c.Endpoints.RoutePrefix = "api";
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
    c.Versioning.DefaultVersion = 1;
}).UseSwaggerGen();

app.Run();
