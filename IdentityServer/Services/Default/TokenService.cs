using IdentityServer.Storage.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly IIdGenerator _idGenerator;
        private readonly IReferenceTokenStore _referenceTokenStore;
        private readonly IRefreshTokenStore _refreshTokenStore;
        private readonly IJwtTokenService _jwtTokenService;

        public TokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            IReferenceTokenStore referenceTokenService,
            IRefreshTokenStore refreshTokenStore,
            IJwtTokenService jwtTokenService)
        {
            _clock = clock;
            _idGenerator = idGenerator;
            _refreshTokenStore = refreshTokenStore;
            _referenceTokenStore = referenceTokenService;
            _jwtTokenService = jwtTokenService;
        }



        public async Task<string> CreateAccessTokenAsync(Token token)
        {
            string securityToken;
            if (token.Type == TokenTypes.AccessToken)
            {
                if (token.AccessTokenType == AccessTokenType.Jwt)
                {
                    securityToken = await _jwtTokenService.CreateTokenAsync(token);
                }
                else
                {
                    await _referenceTokenStore.StoreTokenAsync(token);
                    securityToken = token.Id;
                }
            }
            else if (token.Type == TokenTypes.IdentityToken)
            {
                securityToken = await _jwtTokenService.CreateTokenAsync(token);
            }
            else
            {
                throw new InvalidOperationException("Invalid token type.");
            }
            return securityToken;
        }

        public async Task<Token> CreateTokenAsync(string type, Client client, ClaimsPrincipal subject)
        {
            var id = await _idGenerator.GenerateAsync();
            var lifetime = -1;
            if (type == TokenTypes.AccessToken)
            {
                lifetime = client.AccessTokenLifetime;
            }
            else if (type == TokenTypes.RefreshToken)
            {
                lifetime = client.RefreshTokenLifetime;
            }
            else if (type == TokenTypes.IdentityToken)
            {
                lifetime = client.RefreshTokenLifetime;
            }
            else
            {
                throw new InvalidOperationException($"Invalid token type {type}");
            }
            return new Token(id, type, lifetime, client.AccessTokenType, subject.Claims);
        }

        public async Task<string> CreateRefreshTokenAsync(Token token, int refreshTokenLifetime)
        {
            var id = await _idGenerator.GenerateAsync();
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
