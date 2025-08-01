using Api.Endpoints.Auth.RefreshToken;
using Api.Models;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Api.Endpoints.Auth.Login
{
    public class Endpoint(UserManager<ApplicationUser> userManager):Endpoint<Request,TokenResponse>
    {
        public override void Configure()
        {
            Post("auth/login");
            AllowAnonymous();
            Description(x => x.AutoTagOverride("User"));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if(user is not null && await userManager.CheckPasswordAsync(user, req.Password))
            {
                var roles =await userManager.GetRolesAsync(user);

                Response = await CreateTokenWith<FastEndpointsTokenService>(user.Id.ToString(), u =>
                {
                    u.Roles.AddRange(roles);
                    u.Claims.Add(new Claim("UserId", user.Id.ToString()));
                    u.Claims.Add(new Claim("Username", user.UserName ?? string.Empty));
                });
            }
        }
    }
}
