using FastEndpoints;

namespace Api.Endpoints.Auth.Profile
{
    public class Request
    {
        [FromClaim(false)]
        public string? Username { get; set; }
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
            Get("auth/profile");
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
