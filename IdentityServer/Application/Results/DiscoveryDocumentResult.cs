using IdentityServer.Hosting.Routing;
using IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class DiscoveryDocumentResult 
        : IEndpointResult
    {
        private readonly Dictionary<string, object> _document;

        public DiscoveryDocumentResult(
            Dictionary<string, object> document)
        {
            _document = document;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(_document, ObjectSerializer.JsonSerializerOptions);
        }
    }
}
