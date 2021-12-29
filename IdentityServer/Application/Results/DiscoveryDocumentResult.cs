using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.Configuration;
using IdentityServer.Hosting.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Application
{
    internal class DiscoveryDocumentResult 
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
            var options = context.RequestServices.GetRequiredService<IdentityServerOptions>();
            if (options.Discovery.ResponseCacheInterval.HasValue && options.Discovery.ResponseCacheInterval.Value >= 0)
            {
                context.SetCache(options.Discovery.ResponseCacheInterval.Value, "Origin");
            }
            await context.Response.WriteAsJsonAsync(_document, options.JsonSerializerOptions);
        }
    }
}
