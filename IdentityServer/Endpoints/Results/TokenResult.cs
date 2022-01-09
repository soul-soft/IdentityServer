using System.Net.Mime;
using IdentityServer.Hosting;
using IdentityServer.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class TokenResult : IEndpointResult
    {
        private readonly TokenResponse _response;

        public TokenResult(TokenResponse response)
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
