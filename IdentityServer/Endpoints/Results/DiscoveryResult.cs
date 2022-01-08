using System.Net.Mime;
using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class DiscoveryResult : IEndpointResult
    {
        private readonly DiscoveryResponse _discovery;

        public DiscoveryResult(DiscoveryResponse discovery)
        {
            _discovery = discovery;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var document = _discovery.Write();
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(document, System.Text.Encoding.UTF8);
        }
    }
}
