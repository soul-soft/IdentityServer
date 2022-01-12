using Microsoft.AspNetCore.Http;
using System.Net;

namespace IdentityServer.Endpoints
{
    public class ErrorResult : IEndpointResult
    {
        public string Error { get; set; }

        public string? ErrorDescription { get; set; }

        public HttpStatusCode StatusCode { get; }

        public ErrorResult(string error, HttpStatusCode statusCode)
        {
            Error = error;
            StatusCode = statusCode;
        }

        public ErrorResult(string error, string? errorDescription, HttpStatusCode statusCode)
        {
            Error = error;
            ErrorDescription = errorDescription;
            StatusCode = statusCode;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)StatusCode;
            context.Response.WriteAsJsonAsync(new
            {
                error = Error,
                error_description = ErrorDescription
            });
            return Task.CompletedTask;
        }
    }
}
