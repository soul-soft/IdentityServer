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
        private readonly IUrlParse _serverUrl;
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(
            IUrlParse serverUrl,
            ITokenService tokenService)
        {
            _serverUrl = serverUrl;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ProcessAsync(IClient client)
        {
            var tokenRequest = new TokenRequest()
            {
                Issuer = _serverUrl.GetIdentityServerIssuer(),
                Lifetime = 3600
            };

            tokenRequest.Claims.Add(new Claim(JwtClaimTypes.Subject, "1212"));

            var accessToken = await _tokenService.CreateAccessTokenAsync(tokenRequest);

            var response = new TokenResponse()
            {
                AccessToken = accessToken
            };

            return response;
        }
    }
}
