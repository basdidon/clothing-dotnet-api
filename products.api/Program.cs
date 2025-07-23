using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Products.Api;
using Products.Api.Consumers;
using Products.Api.Extensions;
using Products.Api.Persistance;
using SharedLibrary.Masstransit;
using SharedLibrary.Persistance.Extensions;
using SharedLibrary.Settings;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    var environment = builder.Environment;
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    if(environment.IsDevelopment())
    {
        opts.UseCustomAsyncSeeding();
    }
});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<GetProductsRequestHandler>();
 
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.ConnectConsumeObserver(new LoggingConsumeObserver());
        configurator.UseDelayedMessageScheduler();
        var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;
        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });

        /*
        configurator.ReceiveEndpoint("rollback_submit_product_queue", e =>
        {
            e.ConfigureConsumer<RollbackSubmitProductConsumer>(context);
        });*/


        configurator.ConfigureEndpoints(context);
    });
});
       
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(["http://localhost:3000"]);
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
           s.Title = "Product API";
           s.Version = "v1";
       };
   });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync<ApplicationDbContext>(resetOnStart:true);
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