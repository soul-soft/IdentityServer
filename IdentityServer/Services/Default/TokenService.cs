using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly ITokenStore _tokenStore;
        private readonly IIdGenerator _idGenerator;
        private readonly IJwtTokenService _jwtTokenService;

        public TokenService(
            ISystemClock clock,
            IIdGenerator idGenerator,
            ITokenStore tokenService,
            IJwtTokenService jwtTokenService)
        {
            _clock = clock;
            _idGenerator = idGenerator;
            _tokenStore = tokenService;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> CreateAccessTokenAsync(AccessTokenType accessTokenType, int lifetime, IEnumerable<string> algorithms, IEnumerable<Claim> claims)
        {
            string accessToken;
            var id = await _idGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var token = new Token(
                id:id, 
                type:TokenTypes.AccessToken,
                lifetime:lifetime,
                claims:claims, 
                creationTime: creationTime);
            if (accessTokenType == AccessTokenType.Jwt)
            {
                accessToken = await _jwtTokenService.CreateJwtTokenAsync(token, algorithms);
            }
            else
            {
                await _tokenStore.SaveTokenAsync(token);
                accessToken = token.Id;
            }
            return accessToken;
        }

        public async Task<string> CreateRefreshTokenAsync(IEnumerable<Claim> claims, int lifetime)
        {
            var id = await _idGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var token = new Token(
                 id: id,
                 type: TokenTypes.RefreshToken,
                 lifetime: lifetime,
                 claims: claims,
                 creationTime: creationTime);
            await _tokenStore.SaveTokenAsync(token);
            return id;
        }
    }
}
