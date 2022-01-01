using IdentityServer.Hosting;
using IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class TokenResult : IEndpointResult
    {
        public Dictionary<string, object> _entities = new Dictionary<string, object>();

        public TokenResult(Dictionary<string, object> entities)
        {
            _entities = entities;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            await context.Response.WriteAsJsonAsync(_entities, ObjectSerializer.JsonSerializerOptions);
        }
    }
}
