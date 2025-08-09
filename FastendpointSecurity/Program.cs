using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);

builder.Services
   .AddAuthenticationCookie(validFor: TimeSpan.FromDays(1), o =>
   {
       o.Cookie.Name = "token";
       o.Cookie.SameSite = SameSiteMode.None; // ✅ Needed for cross-site
       o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   }) //configure cookie auth
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

// must call first
app.UseCors(MyAllowSpecificOrigins);
app.UseStaticFiles();

app//.UseJwtRevocation<BlacklistChecker>()
    .UseAuthentication()
    .UseAuthorization()
    .UseAntiforgeryFE()
    .UseFastEndpoints(c =>
    {
        /*c.Endpoints.Configurator = ep =>
        {
            ep.Options(b => b.AddEndpointFilter<EndpointRequestFilter>());
        };*/
        c.Endpoints.RoutePrefix = "api";
        c.Versioning.Prefix = "v";
        c.Versioning.PrependToRoute = true;
        c.Versioning.DefaultVersion = 1;
    }).UseSwaggerGen();

app.MapGet("/", () => "Hello World!");

app.Run();
