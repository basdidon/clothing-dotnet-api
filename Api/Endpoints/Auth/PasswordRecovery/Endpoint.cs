using FastEndpoints;
using FastEndpoints.Swagger;

namespace Api.Endpoints.Auth.PasswordRecovery
{
    public class Endpoint : Endpoint<Request>
    {
        public override void Configure()
        {
            Post("auth/password-recovery");
            AllowAnonymous();
            Description(x => x.AutoTagOverride("User"));
        }

        public override Task HandleAsync(Request req, CancellationToken ct)
        {
            // send otp to email
            throw new NotImplementedException();
        }
    }
}
