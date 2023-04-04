using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResult : IEndpointResult
    {
        private readonly DiscoveryGeneratorResponse _discovery;

        public DiscoveryResult(DiscoveryGeneratorResponse discovery)
        {
            _discovery = discovery;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var json = _discovery.Serialize();
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
        }
    }
}
