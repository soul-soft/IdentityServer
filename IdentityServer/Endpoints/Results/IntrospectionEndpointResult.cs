using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace IdentityServer.Endpoints
{
    internal class IntrospectionEndpointResult : IEndpointResult
    {
        private readonly IntrospectionGeneratorResponse _response;

        public IntrospectionEndpointResult(IntrospectionGeneratorResponse response)
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
