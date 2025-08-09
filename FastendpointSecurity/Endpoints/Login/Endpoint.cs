using FastEndpoints;
using FastEndpoints.Security;

namespace FastendpointSecurity.Endpoints.Login
{
    public class Endpoint:EndpointWithoutRequest
    {
        public override void Configure()
        {
            Post("login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await CookieAuth.SignInAsync(u =>
            {
                u.Roles.Add("Admin");
                //u.Permissions.AddRange(["Create_Item", "Delete_Item"]);
                //u.Claims.Add(new("Address", "123 Street"));

                //indexer based claim setting
                u["Email"] = "abc@def.com";
                //u["Department"] = "Administration";
            });
        }
    }
}
