using System.Net;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class StatusCodeResult : IEndpointResult
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
