using IdentityServer.Hosting;
using IdentityServer.Infrastructure;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class DiscoveryKeyResult : IEndpointResult
    {
        public IEnumerable<JsonWebKey> JsonWebKeys { get; }

        public DiscoveryKeyResult(IEnumerable<JsonWebKey> jsonWebKeys)
        {
            JsonWebKeys = jsonWebKeys;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var data = new 
            { 
                keys = JsonWebKeys
            };
            await context.Response.WriteAsJsonAsync(data, ObjectSerializer.JsonSerializerOptions);
        }
    }
}
