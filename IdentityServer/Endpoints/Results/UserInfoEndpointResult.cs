using System.Net.Mime;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class UserInfoEndpointResult : IEndpointResult
    {
        private readonly UserInfoGeneratorResponse _response;

        public UserInfoEndpointResult(UserInfoGeneratorResponse response)
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
