using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace IdentityServer.Endpoints
{
    public class BadRequestResult : IEndpointResult
    {
        public string Error { get; set; }

        public string? ErrorDescription { get; set; }

        public BadRequestResult(string error, string? errorDescription)
        {
            Error = error;
            ErrorDescription = errorDescription;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var options = context.RequestServices
                .GetRequiredService<IdentityServerOptions>();
            if (!options.IncludeEndpointErrorDetails)
            {
                ErrorDescription = null;
            }
            context.Response.WriteAsJsonAsync(new
            {
                error = Error,
                error_description = ErrorDescription
            });
            return Task.CompletedTask;
        }
    }
}
