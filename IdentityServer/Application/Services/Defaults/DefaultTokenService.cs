using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IdentityModel;
using IdentityServer.Hosting;
using IdentityServer.Storage;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class DefaultTokenService
        : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IServerUrls _urls;
        private readonly ITokenCreationService _creationService;
        private readonly ISigningCredentialStore _credentials;

        public DefaultTokenService(
            IServerUrls urls,
            ISystemClock clock,
            ISigningCredentialStore credentials,
            ITokenCreationService creationService)
        {
            _urls = urls;
            _clock = clock;
            _credentials = credentials;
            _creationService = creationService;
        }

        public Task<Token> CreateAccessTokenAsync(TokenCreationRequest request)
        {
            var claims = new List<Claim>();
            if (request.Client.IncludeJwtId)
            {
                claims.Add(new Claim(JwtClaimTypes.JwtId, CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)));
            }
            if (!string.IsNullOrWhiteSpace(request.SessionId))
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, request.SessionId));
            }
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, _clock.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
            var issuer = _urls.GetIdentityServerIssuerUri();
            var token = new Token
            {
                CreationTime = _clock.UtcNow.UtcDateTime,
                Issuer = issuer,
                Lifetime = request.AccessTokenLifetime,
                Claims = claims.Distinct(new ClaimComparer()).ToList(),
                ClientId = request.Client.ClientId,
                Description = request.Description,
                AccessTokenType = request.AccessTokenType,
                AllowedSigningAlgorithms = request.Resources.ApiResources.FindMatchingSigningAlgorithms()
            };
            return Task.FromResult(token);
          
        }

        public Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
        {
            var token = new Token
            {
            };

            return Task.FromResult(token);
        }

        public Task<string> CreateSecurityTokenAsync(Token token)
        {
            return _creationService.CreateTokenAsync(token);
        }

    }
}
