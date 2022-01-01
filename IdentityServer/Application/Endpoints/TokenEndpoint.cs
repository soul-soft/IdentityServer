using IdentityServer.Hosting;
using IdentityServer.Models;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Application
{
    internal class TokenEndpoint : IEndpointHandler
    {
        private readonly ITokenService _tokenService;

        private readonly IServerUrls _urls;

        public TokenEndpoint(ITokenService tokenService, IServerUrls serverUrls)
        {
            _tokenService = tokenService;
            _urls = serverUrls;
        }

        public async Task<IEndpointResult> ProcessAsync(HttpContext context)
        {
            var token = await _tokenService.CreateAccessTokenAsync(new TokenCreationRequest
            {
                AccessTokenLifetime = 3600,
                AccessTokenType = AccessTokenType.Jwt,
                Client = new Client(),
                Issuer = _urls.GetIdentityServerIssuerUri(),
                Resources = new Resources()
            });
            var jwt = await _tokenService.CreateSecurityTokenAsync(token);
            var data = new Dictionary<string, object>();
            data.Add("access_token", jwt);
            return new TokenResult(data);
        }
    }
}
