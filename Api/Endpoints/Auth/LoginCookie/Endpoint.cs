using Api.Models;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;

namespace Api.Endpoints.Auth.LoginCookie
{
    public class Request
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Endpoint (UserManager<ApplicationUser> userManager): Endpoint<Request>
    {
        public override void Configure()
        {
            Post("/auth/cookie/login");
            AllowAnonymous();
            Description(x => x.AutoTagOverride("User"));
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is not null && await userManager.CheckPasswordAsync(user, req.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                await CookieAuth.SignInAsync(u =>
                {
                    u.Roles.AddRange(roles);
                    //u.Permissions.AddRange(["Create_Item", "Delete_Item"]);
                    //u.Claims.Add(new("Address", "123 Street"));

                    //indexer based claim setting
                    u["Email"] = req.Email;
                    //u["Department"] = "Administration";
                });
            }
        }
    }
}
