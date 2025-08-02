using FastEndpoints;
using System.Security.Claims;

namespace Api.Endpoints.Auth.Profile
{
    public class Request
    {
        [FromClaim]
        public string Username { get; set; } =string.Empty;
        [FromClaim]
        public string Email { get; set; } = string.Empty;
        [FromClaim("role")]
        public string[] Roles { get; set; } = [];

    }

    public class Response
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string[] Roles { get; set; } = [];
    }

    public class Endpoint:Endpoint<Request,Response>
    {
        public override void Configure()
        {
            Get("auth/profile/me");
        }

        public override Task HandleAsync(Request req,CancellationToken ct)
        {

            Response = new()
            {
                Username = req.Username,
                Email = req.Email,
                Roles = req.Roles
            };

            return Task.CompletedTask;
        }
    }
}
