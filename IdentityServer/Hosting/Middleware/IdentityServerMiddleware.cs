using Microsoft.AspNetCore.Http;

namespace IdentityServer.Hosting
{
    internal class IdentityServerMiddleware
    {
        private readonly RequestDelegate _next;

        public IdentityServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IEndpointRouter router)
        {
            var endpoint = router.Find(context);
            if (endpoint == null)
            {
                await _next(context);
                return;
            }
            var result = await endpoint.ProcessAsync(context);
            await result.ExecuteAsync(context);
        }        
    }
}
