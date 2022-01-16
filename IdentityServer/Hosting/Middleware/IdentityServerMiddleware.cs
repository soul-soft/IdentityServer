using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Hosting
{
    internal class IdentityServerMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<IdentityServerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is InvalidException validationException)
                {
                    var result = new ErrorResult(
                        validationException.Error,
                        validationException.ErrorDescription,
                        HttpStatusCode.BadRequest);
                    await result.ExecuteAsync(context);
                }
                else
                {
                    var result = new ErrorResult(
                       OpenIdConnectTokenErrors.InvalidRequest,
                       HttpStatusCode.BadRequest);
                    await result.ExecuteAsync(context);
                }
                logger.LogError(ex, ex.Message);
            }

        }
    }
}
