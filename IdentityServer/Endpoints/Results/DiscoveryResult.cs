using IdentityServer.Hosting;
using IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResult : IEndpointResult
    {
        private readonly DiscoveryResponse _response;

        public DiscoveryResult(DiscoveryResponse response)
        {
            _response = response;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(_response, ObjectSerializer.JsonSerializerOptions);
        }
    }
}
