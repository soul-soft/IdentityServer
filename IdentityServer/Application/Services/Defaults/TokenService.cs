using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Application
{
    internal class TokenService
        : ITokenService
    {
        private readonly ISystemClock _clock;

        public TokenService(
            ISystemClock clock)
        {
            _clock = clock;
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
            var token = new Token(request.Issuer, OidcConstants.TokenTypes.AccessToken)
            {
                CreationTime = _clock.UtcNow.UtcDateTime,                
                Claims = claims.Distinct(new ClaimComparer()).ToHashSet(),
                AccessTokenType = request.Client.AccessTokenType,
            };
            if (request.Client.AccessTokenLifetime.HasValue)
            {
                token.Expires = _clock.UtcNow.UtcDateTime
                    .AddSeconds(request.Client.AccessTokenLifetime.Value);
            }
            return Task.FromResult(token);
          
        }

        public Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request)
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
            var token = new Token(request.Issuer, OidcConstants.TokenTypes.IdentityToken)
            {
                CreationTime = _clock.UtcNow.UtcDateTime,
                Claims = claims.Distinct(new ClaimComparer()).ToHashSet(),
                AccessTokenType = request.Client.AccessTokenType,
            };
            if (request.Client.AccessTokenLifetime.HasValue)
            {
                token.Expires = _clock.UtcNow.UtcDateTime
                    .AddSeconds(request.Client.AccessTokenLifetime.Value);
            }
            return Task.FromResult(token);
        }
    }
}
