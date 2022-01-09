using IdentityServer.Endpoints;
using IdentityServer.Models;
using IdentityServer.Services;

namespace IdentityServer.ResponseGenerators
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ISecurityTokenService _tokenService;

        public TokenResponseGenerator(ISecurityTokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ProcessAsync(IClient client)
        {
            var tokenRequest = new SecurityTokenRequest();

            var accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);

            var response = new TokenResponse()
            {
                AccessToken = accessToken
            };

            return response;
        }
    }
}
