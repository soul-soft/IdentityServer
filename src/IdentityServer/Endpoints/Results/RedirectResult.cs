using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class RedirectResult : IEndpointResult
    {
        private readonly string _url;
       
        public RedirectResult(string url)
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
