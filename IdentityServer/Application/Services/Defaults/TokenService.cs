using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Application
{
    internal class TokenService
        : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IClaimsService _claimsService;
        private readonly ITokenCreationService _creation;

        public TokenService(
            ISystemClock clock,
            IClaimsService claimsService,
            ITokenCreationService creation)
        {
            _clock = clock;
            _creation = creation;
            _claimsService = claimsService;
        }

        public async Task<Token> CreateAccessTokenAsync(TokenRequest request)
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
            foreach (var scope in request.Scopes)
            {
                claims.Add(new Claim(JwtClaimTypes.Scope, scope));
            }
            var claimsRequest = new ClaimsRequest(request.Client, request.UserClaims);
            var issuerClaims = await _claimsService.GetAccessTokenClaimsAsync(claimsRequest);
            foreach (var item in issuerClaims)
            {
                claims.Add(item);
            }
            var token = new Token(OidcConstants.TokenTypes.IdentityToken, request.Issuer, request.Scopes)
            {
                AccessTokenType = request.Client.AccessTokenType,
                Lifetime = request.Client.IdentityTokenLifetime,
                Claims = claims.ToHashSet()
            };
            return token;
        }

        public async Task<Token> CreateIdentityTokenAsync(TokenRequest request)
        {
            var claims = new HashSet<Claim>();
            if (request.Client.IncludeJwtId)
            {
                claims.Add(new Claim(JwtClaimTypes.JwtId, CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)));
            }
            if (!string.IsNullOrWhiteSpace(request.SessionId))
            {
                claims.Add(new Claim(JwtClaimTypes.SessionId, request.SessionId));
            }
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, _clock.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64));
            var claimsRequest = new ClaimsRequest(request.Client, request.UserClaims);
            var issuerClaims = await _claimsService.GetIdentityTokenClaimsAsync(claimsRequest);
            foreach (var item in issuerClaims)
            {
                claims.Add(item);
            }
            var token = new Token(OidcConstants.TokenTypes.IdentityToken, request.Issuer, request.Scopes)
            {
                AccessTokenType = request.Client.AccessTokenType,
                Lifetime = request.Client.IdentityTokenLifetime,
                Claims = claims
            };
            return token;
        }

        public async Task<string> CreateSecurityTokenAsync(Token token)
        {
            if (token.Type == OidcConstants.TokenTypes.AccessToken)
            {
                return await _creation.CreateTokenAsync(token);
            }
            else if (token.Type == OidcConstants.TokenTypes.IdentityToken)
            {
                return await _creation.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
        }
    }
}
