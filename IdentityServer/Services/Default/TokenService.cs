using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
using System.Net;
using System.Security.Claims;
using System.Web;

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

        public async Task<string> CreateAccessTokenAsync(AccessTokenType accessTokenType, int lifetime, IEnumerable<string> algorithms, IEnumerable<Claim> claims)
        {
            string accessToken;
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var token = new Token(
                id: id,
                type: TokenTypes.AccessToken,
                lifetime: lifetime,
                claims: claims,
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
            var id = await _randomGenerator.GenerateAsync();
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
