using FastEndpoints;

namespace FastendpointSecurity.Endpoints.Restricted
{
    public class Endpoint:EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("restricted");
            Roles("Admin");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Send.OkAsync("you are admin!!",ct);
        }
    }
}
