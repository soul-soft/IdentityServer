using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly ITokenStore _tokenStore;
        private readonly IRandomGenerator _randomGenerator;
        private readonly ISecurityTokenService _jwtTokenService;

        public TokenService(
            ISystemClock clock,
            ITokenStore tokenService,
            IRandomGenerator randomGenerator,
            ISecurityTokenService jwtTokenService)
        {
            _clock = clock;
            _tokenStore = tokenService;
            _randomGenerator = randomGenerator;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject)
        {
            string accessToken;
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var token = new Token(
                id: id,
                type: TokenTypes.AccessToken,
                lifetime: client.AccessTokenLifetime,
                claims: subject.Claims,
                creationTime: creationTime);
            if (client.AccessTokenType == AccessTokenType.Jwt)
            {
                accessToken = await _jwtTokenService.CreateJwtTokenAsync(token, client.AllowedSigningAlgorithms);
            }
            else
            {
                await _tokenStore.SaveTokenAsync(token);
                accessToken = token.Id;
            }
            return accessToken;
        }

        public async Task<string> CreateRefreshTokenAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var token = new Token(
                 id: id,
                 type: TokenTypes.RefreshToken,
                 lifetime: client.RefreshTokenLifetime,
                 claims: subject.Claims,
                 creationTime: creationTime);
            await _tokenStore.SaveTokenAsync(token);
            return id;
        }
    }
}
