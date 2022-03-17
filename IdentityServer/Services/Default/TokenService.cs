using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly ITokenStore _referenceTokenStore;
        private readonly IHandleGenerator _handleGenerator;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly ISecurityTokenService _securityTokenService;

        public TokenService(
            ISystemClock clock,
            IHandleGenerator handleGenerator,
            ITokenStore referenceTokenService,
            IRefreshTokenStore refreshTokenStore,
            ISecurityTokenService securityTokenService)
        {
            _clock = clock;
            _handleGenerator = handleGenerator;
            _refreshTokenStore = refreshTokenStore;
            _referenceTokenStore = referenceTokenService;
            _securityTokenService = securityTokenService;
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
        public async Task<Token> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _handleGenerator.GenerateAsync();
            var token = new Token(
                id,
                TokenTypes.AccessToken,
                client.AccessTokenType,
                subject.Claims,
                client.AllowedSigningAlgorithms);
            return token;
        }

        public async Task<string> CreateRefreshTokenAsync(Token token, int refreshTokenLifetime)
        {
            var id = await _handleGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var refreshToken = new RefreshToken(
                id, 
                token, 
                refreshTokenLifetime, 
                creationTime);
            await _refreshTokenStore.StoreRefreshTokenAsync(refreshToken);
            return id;
        }
    }
}
