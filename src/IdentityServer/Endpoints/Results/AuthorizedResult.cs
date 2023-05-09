using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class AuthorizedResult : IEndpointResult
    {
        private readonly string _url;
       
        public AuthorizedResult(string url)
        {
            _url = url;
        }
        
        public Task ExecuteAsync(HttpContext context)
        {
            context.Response.Redirect(_url);
            return Task.CompletedTask;
        }
    }
}
