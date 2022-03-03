using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IClaimsService _claimsService;
        private readonly IHandleGenerator _identifyGenerator;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly ITokenStore _referenceTokenStore;
        private readonly ISecurityTokenService _securityTokenService;

        public TokenService(
            ISystemClock clock,
            IClaimsService claimsService,
            IHandleGenerator identifyGenerator,
            IRefreshTokenStore refreshTokenStore,
            ITokenStore referenceTokenService,
            ISecurityTokenService securityTokenService)
        {
            _clock = clock;
            _claimsService = claimsService;
            _refreshTokenStore = refreshTokenStore;
            _identifyGenerator = identifyGenerator;
            _referenceTokenStore = referenceTokenService;
            _securityTokenService = securityTokenService;
        }

        public async Task<Token> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            var id = await _identifyGenerator.GenerateAsync();
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request);
            var token = new Token
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                GrantType = request.GrantType,
                Claims = claims.ToList(),
                Lifetime = request.Client.AccessTokenLifetime,
                CreationTime = _clock.UtcNow.UtcDateTime,
                AccessTokenType = request.Client.AccessTokenType,
            };
            return token;
        }

        public async Task<Token> CreateIdentityTokenAsync(ValidatedTokenRequest request)
        {
            var id = await _identifyGenerator.GenerateAsync();
            var claims = await _claimsService.GetIdentityTokenClaimsAsync(request);
            var token = new Token
            {
                Id = id,
                Type = TokenTypes.AccessToken,
                GrantType = request.GrantType,
                Claims = claims.ToList(),
                Lifetime = request.Client.AccessTokenLifetime,
                CreationTime = _clock.UtcNow.UtcDateTime,
                AccessTokenType = request.Client.AccessTokenType,
            };
            return token;
        }

        public async Task<string> CreateSecurityTokenAsync(Token token)
        {
            string securityToken;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    securityToken = await _securityTokenService.CreateTokenAsync(token);
                }
                else
                {
                    await _referenceTokenStore.StoreTokenAsync(token);
                    securityToken = token.Id;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {
                securityToken = await _securityTokenService.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return securityToken;
        }

        public async Task<string> CreateRefreshTokenAsync(Token token, int lifetime)
        {
            var id = await _identifyGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var refreshToken = new RefreshToken(id, token, lifetime, creationTime);
            await _refreshTokenStore.StoreRefreshTokenAsync(refreshToken);
            return id;
        }

    }
}
