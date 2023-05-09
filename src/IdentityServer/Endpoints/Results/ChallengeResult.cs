using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Endpoints
{
    internal class ChallengeResult : IEndpointResult
    {
        public async Task ExecuteAsync(HttpContext context)
        {
            var redirectUri = $"{context.Request.Path}{context.Request.QueryString}";
            await context.ChallengeAsync(new AuthenticationProperties()
            {
                RedirectUri = redirectUri,
            });
        }
    }
}
