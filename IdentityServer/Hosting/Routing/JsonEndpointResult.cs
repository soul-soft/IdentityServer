using IdentityServer.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Hosting.Routing
{
    public class JsonEndpointResult
        : IEndpointResult
    {
        private readonly object _obj;

        public JsonEndpointResult(object obj)
        {
            _obj = obj;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var options = context.RequestServices
                .GetRequiredService<IdentityServerOptions>();
            await context.Response.WriteAsJsonAsync(_obj, options.JsonSerializerOptions);
        }
    }
}
