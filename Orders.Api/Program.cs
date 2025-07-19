using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.Extensions.Options;
using SharedLibrary.FastEndpoint.Filters;
using SharedLibrary.Masstransit;
using SharedLibrary.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddMassTransit(x =>
{
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

builder.Services
   .AddFastEndpoints(o => o.IncludeAbstractValidators = true)
   .SwaggerDocument(o =>
   {
       o.MaxEndpointVersion = 1;
       o.DocumentSettings = s =>
       {
           s.DocumentName = "Initial Release";
           s.Title = "Order API";
           s.Version = "v1";
       };
   });

var app = builder.Build();

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
