using FastEndpoints.Security;

namespace Products.Api
{
    public class BlacklistChecker(RequestDelegate next, ILogger<BlacklistChecker> logger) : JwtRevocationMiddleware(next)
    {
        protected override Task<bool> JwtTokenIsValidAsync(string jwtToken, CancellationToken ct)
        {
            logger.LogInformation("jwtToken : {}", jwtToken);
            return Task.FromResult(true);
        }
    }
}