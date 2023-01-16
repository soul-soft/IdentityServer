using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    public class AuthorizeResult : IEndpointResult
    {
        private readonly AuthorizeGeneratorResponse _response;
        
        public AuthorizeResult(AuthorizeGeneratorResponse response)
        {
            _response = response;
        }

        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.Redirect($"{_response.RedirectUri}?code={_response.Code}");
            return Task.CompletedTask;
        }
    }
}
