using IdentityServer.Endpoints;
using IdentityServer.Models;
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

        public async Task<TokenResponse> ProcessAsync(TokenCreationRequest request)
        {
            var at = await _tokenService.CreateAccessTokenAsync(request);

            var accessToken = await _tokenService.CreateSecurityTokenAsync(at);

            var scope = string.Join(",", request.Scopes);
           
            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                AccessTokenLifetime = request.Client.AccessTokenLifetime,
                Scope = scope,
            };

            return response;
        }
    }
}
