using Api;
using Api.Extensions;
using Api.Models;
using Api.Persistance;
using Api.Services;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stripe;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:Apikey").Value!;

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
var signingKey = builder.Configuration.GetSection("jwt:signingKey").Value!;


builder.Services.AddTransient<RoleService>();
builder.Services.AddTransient<PaymentIntentService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connection);
});
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireDigit = false;
    })
    .AddRoles<ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services.AddAuthenticationCookie(validFor: TimeSpan.FromMinutes(10), o =>
{
    o.Cookie.Name = "token";
    o.Cookie.SameSite = SameSiteMode.None; // cross-site support
    o.Cookie.SecurePolicy = CookieSecurePolicy.Always; // or None for dev http
})
    .AddAuthorization()
    .AddAntiforgery()
    .AddFastEndpoints(o => o.IncludeAbstractValidators = true)
    .SwaggerDocument(o =>
    {
        o.MaxEndpointVersion = 1;
        o.DocumentSettings = s =>
        {
            s.DocumentName = "Initial Release";
            s.Title = "API v1";
            s.Version = "v1";
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.SeedDatabase();
}
// must call first
app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();

app//.UseJwtRevocation<BlacklistChecker>()
    .UseAuthentication()
    .UseAuthorization()
    .UseAntiforgeryFE()
    .UseFastEndpoints(c =>
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
