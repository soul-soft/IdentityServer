using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    internal class IdentityServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IEndpointRouter _router;

        public IdentityServerMiddleware(
            RequestDelegate next,
            IEndpointRouter router)
        {
            _next = next;
            _router = router;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var handler = _router.Routing(context);
            if (handler != null)
            {
                await ExecuteAsync(context, handler);
            }
            else
            {
                await _next(context);
            }
        }

        private static async Task ExecuteAsync(HttpContext context, IEndpointHandler endpoint)
        {
            try
            {
                var result = await endpoint.ProcessAsync(context);
                await result.ExecuteAsync(context);
            }
            catch (ValidationException ex)
            {
                var result = new BadRequestResult(ex.Error, ex.ErrorDescription);
                await result.ExecuteAsync(context);
            }
            catch (UnauthorizedException ex)
            {
                var result = new UnauthorizedResult(ex.Error, ex.ErrorDescription);
                await result.ExecuteAsync(context);
            }
        }
    }
}
