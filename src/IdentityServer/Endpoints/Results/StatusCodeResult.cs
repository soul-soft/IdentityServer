using Microsoft.AspNetCore.Http;
using System.Net;

namespace IdentityServer.Endpoints
{
    internal class StatusCodeResult : IEndpointResult
    {
        public HttpStatusCode StatusCode { get; }

        public StatusCodeResult(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)StatusCode;
            return Task.CompletedTask;
        }
    }
}
