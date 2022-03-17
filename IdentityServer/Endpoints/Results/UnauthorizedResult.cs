using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Net;

namespace IdentityServer.Endpoints
{
    public class UnauthorizedResult : IEndpointResult
    {
        public string Error { get; }

        public string? ErrorDescription { get; }

        public UnauthorizedResult(string error, string? errorDescription = null)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            if (Error == OpenIdConnectValidationErrors.InsufficientScope)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
            var errorString = string.Format($"error=\"{Error}\"");
            if (string.IsNullOrEmpty(ErrorDescription))
            {
                context.Response.Headers.Add(HeaderNames.WWWAuthenticate, new StringValues(new[] { "Bearer realm=\"IdentityServer\"", errorString }).ToString());
            }
            else
            {
                var errorDescriptionString = string.Format($"error_description=\"{ErrorDescription}\"");
                context.Response.Headers.Add(HeaderNames.WWWAuthenticate, new StringValues(new[] { "Bearer realm=\"IdentityServer\"", errorString, errorDescriptionString }).ToString());
            }
            return Task.CompletedTask;
        }
    }
}
