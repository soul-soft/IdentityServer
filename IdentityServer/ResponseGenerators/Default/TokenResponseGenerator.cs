using System.Security.Claims;
using IdentityServer.Endpoints;
using IdentityServer.Hosting;
using IdentityServer.Models;
using IdentityServer.Protocols;
using IdentityServer.Services;

namespace IdentityServer.ResponseGenerators
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISecurityTokenService _tokenService;

        public TokenResponseGenerator(
            IServerUrl serverUrl,
            ISecurityTokenService tokenService)
        {
            _serverUrl = serverUrl;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ProcessAsync(IClient client)
        {
            var tokenRequest = new SecurityTokenRequest()
            {
                Issuer = _serverUrl.GetIdentityServerIssuer(),
                Lifetime = 3600
            };

            tokenRequest.Claims.Add(new Claim(OpenIdConnectClaimTypes.Subject, "1212"));

            var accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);

            var response = new TokenResponse()
            {
                AccessToken = accessToken
            };

            return response;
        }
    }
}
