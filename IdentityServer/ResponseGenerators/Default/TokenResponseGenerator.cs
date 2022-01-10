using System.Security.Claims;
using IdentityServer.Endpoints;
using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.Services;

namespace IdentityServer.ResponseGenerators
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly IServerUrl _serverUrl;
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(
            IServerUrl serverUrl,
            ITokenService tokenService)
        {
            _serverUrl = serverUrl;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ProcessAsync(TokenRequest request)
        {
            var tokenRequest = new TokenCreationRequest(request.Client, request.Resources)
            {
            };
           
            var accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);

            var securityAccessToken = await _tokenService.CreateSecurityTokenAsync(accessToken);

            var response = new TokenResponse()
            {
                AccessToken = securityAccessToken
            };

            return response;
        }
    }
}
