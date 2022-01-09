using System.Net.Mime;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class JwkDiscoveryResult : IEndpointResult
    {
        public JwkDiscoveryResponse _response;

        public JwkDiscoveryResult(JwkDiscoveryResponse response)
        {
            _response = response;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var json = _response.Serialize();
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(json, System.Text.Encoding.UTF8);
        }
    }
}
