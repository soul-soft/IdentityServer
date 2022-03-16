using Microsoft.AspNetCore.Authentication;

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

        public Task<Token> CreateAccessTokenAsync(TokenRequest request)
        {
            var token = new Token(
                TokenTypes.AccessToken,
                request.Client.AccessTokenType,
                request.Subject.Claims,
                request.Client.AllowedSigningAlgorithms);
            return Task.FromResult(token);
        }

        public Task<Token> CreateIdentityTokenAsync(TokenRequest request)
        {
            var token = new Token(
                TokenTypes.IdentityToken,
                request.Client.AccessTokenType, 
                request.Subject.Claims,
                request.Client.AllowedSigningAlgorithms);
            return Task.FromResult(token);
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
                    securityToken = token.JwtId;
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
            var id = await _handleGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var refreshToken = new RefreshToken(id, token, lifetime, creationTime);
            await _refreshTokenStore.StoreRefreshTokenAsync(refreshToken);
            return id;
        }

    }
}
