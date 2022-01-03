using IdentityServer.Hosting;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    public class TokenErrorResult : IEndpointResult
    {
        public readonly TokenErrorResponse _response;

        public TokenErrorResult(TokenErrorResponse response)
        {
            _response = response;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var data = new Dictionary<string, object?>();
            data.Add("error", _response.Error);
            data.Add("error_description", _response.ErrorDescription);
            await context.WriteAsJsonExAsync(data);
        }
    }
}
