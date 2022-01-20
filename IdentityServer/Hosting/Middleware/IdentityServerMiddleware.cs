using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace IdentityServer.Hosting
{
    internal class IdentityServerMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILoggerFactory loggerFactory)
        {
            var endpoint = context.GetEndpoint();
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (endpoint != null && endpoint.IsIdentityEndpoint())
                {
                    if (ex is InvalidException validationException)
                    {
                        var result = new ErrorResult(
                            validationException.Error,
                            validationException.ErrorDescription,
                            HttpStatusCode.BadRequest);
                        await result.ExecuteAsync(context);
                        loggerFactory.CreateLogger("IdentityServer").LogError(ex, ex.Message);
                    }
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
