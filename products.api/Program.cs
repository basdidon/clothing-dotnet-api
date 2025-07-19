using FastEndpoints;
using FastEndpoints.Swagger;
using Products.Api;
using Products.Api.Persistance;
using Microsoft.EntityFrameworkCore;
using Products.Api.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseAsyncSeeding(async (context, _, ct) =>
        {
            Product[] products = [
                new(){
                    Id = Guid.NewGuid(),
                    Title = "Demi",
                    SubTitle = "pink floral embroidered stretch cotton corset top" ,
                    UnitPrice = 109,
                    Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                    AvaliableSizes = AvaliableSizes.All
                },
                new(){
                    Id = Guid.NewGuid(),
                    Title = "Ysabella",
                    SubTitle = "cream rose print stretch cotton mini sundress",
                    UnitPrice = 179,
                    Description = "Lorem ipsum dolor, sit amet consectetur adipisicing elit. Dolorem ad explicabo eaque consectetur, dolore, error nostrum provident autem voluptatum libero ea distinctio veniam eos atque mollitia expedita sequi quo in nobis sapiente minima tempora id sunt? Sint ex corrupti nostrum odio cumque magni tempore similique repudiandae obcaecati?",
                    AvaliableSizes = AvaliableSizes.All
                }
            ];
            await context.Set<Product>().AddRangeAsync(products, ct);
            await context.SaveChangesAsync(ct);
        }));

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
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

app.MapGet("/", () => "Hello World!");

app.Run();