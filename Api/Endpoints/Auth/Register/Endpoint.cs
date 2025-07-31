using Api.Constants;
using Api.Endpoints.Auth.RefreshToken;
using Api.Models;
using Api.Persistance;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Api.Endpoints.Auth.Register
{
    public class Endpoint(UserManager<ApplicationUser> userManager, ApplicationDbContext context) : Endpoint<Request, TokenResponse>
    {
        public override void Configure()
        {
            Post("auth/register");
            AllowAnonymous();
            Description(x => x.AutoTagOverride("User"));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            ApplicationUser user = new()
            {
                Email = req.Email,
                Firstname = req.Firstname,
                Lastname = req.Lastname,
                Country = req.Country,
                State = req.State,
                PhoneNumber = req.PhoneNumber,
            };
            await userManager.CreateAsync(user, req.Password);
            await context.SaveChangesAsync(ct); // required to save user before add role ?
            await userManager.AddToRoleAsync(user, Role.Customer);
            await context.SaveChangesAsync(ct);


            Response = await CreateTokenWith<FastEndpointsTokenService>(user.Id.ToString(), u =>
            {
                u.Roles.Add(Role.Customer);
                u.Claims.Add(new Claim("UserId", user.Id.ToString()));
                u.Claims.Add(new Claim("Username", user.UserName ?? string.Empty));
            });
        }
    }
}
