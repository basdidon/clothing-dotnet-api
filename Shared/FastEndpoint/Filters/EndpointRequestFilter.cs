﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SharedLibrary.FastEndpoint.Filters
{
    // for more information go 
    // https://fast-endpoints.com/docs/configuration-settings#filtering-endpoint-registration
    public class EndpointRequestFilter(ILogger<EndpointRequestFilter> logger) : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Log the incoming request
            var httpContext = context.HttpContext;
            logger.LogInformation("Received request: [{Method}] {Path}", httpContext.Request.Method, httpContext.Request.Path);

            // Call the next delegate in the pipeline
            var result = await next(context);

            return result;

        }
    }
}
