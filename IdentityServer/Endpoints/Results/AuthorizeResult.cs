using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text;

namespace IdentityServer.Endpoints
{
    public class AuthorizeResult : IEndpointResult
    {
        private readonly AuthorizeGeneratorRequest _request;
       
        public AuthorizeResult(AuthorizeGeneratorRequest request)
        {
            _request = request;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            var result = await context.AuthenticateAsync();
            if (!result.Succeeded)
            {
                await ChallengeAsync(context);
            }
            else
            {

            }
        }

        private async Task ChallengeAsync(HttpContext context)
        {
            var redirectUri = $"{context.Request.Path}/{context.Request.QueryString}";
            await context.ChallengeAsync(new AuthenticationProperties() 
            {
                RedirectUri = redirectUri,
            });
        }
    }
}
