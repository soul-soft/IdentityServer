﻿using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class TokenService : ITokenService
    {
        private readonly ISystemClock _clock;
        private readonly ITokenStore _tokenStore;
        private readonly IRandomGenerator _randomGenerator;
        private readonly ISecurityTokenService _jwtTokens;

        public TokenService(
            ISystemClock clock,
            ITokenStore tokenService,
            IRandomGenerator randomGenerator,
            ISecurityTokenService jwtTokens)
        {
            _clock = clock;
            _tokenStore = tokenService;
            _randomGenerator = randomGenerator;
            _jwtTokens = jwtTokens;
        }

        public async Task<string> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject)
        {
            string accessToken;
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var lifetime = client.AccessTokenLifetime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var token = new Token(
                code: id,
                type: TokenTypes.AccessToken,
                lifetime: lifetime,
                claims: subject.Claims.ToArray(),
                expirationTime: expirationTime,
                creationTime: creationTime);
            if (client.AccessTokenType == AccessTokenType.Jwt)
            {
                accessToken = await _jwtTokens.CreateJwtTokenAsync(client, token);
            }
            else
            {
                await _tokenStore.SaveTokenAsync(token);
                accessToken = token.Code;
            }
            return accessToken;
        }

        public async Task<string> CreateRefreshTokenAsync(Client client, ClaimsPrincipal subject)
        {
            var id = await _randomGenerator.GenerateAsync();
            var lifetime = client.RefreshTokenLifetime;
            var creationTime = _clock.UtcNow.UtcDateTime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var token = new Token(
                 code: id,
                 type: TokenTypes.RefreshToken,
                 lifetime: lifetime,
                 claims: subject.Claims.ToArray(),
                 expirationTime: expirationTime,
                 creationTime: creationTime);
            await _tokenStore.SaveTokenAsync(token);
            return id;
        }

        public async Task<string> CreateIdentityTokenAsync(Client client, ClaimsPrincipal subject, AuthorizationCode authorizationCode)
        {
            var id = await _randomGenerator.GenerateAsync();
            var creationTime = _clock.UtcNow.UtcDateTime;
            var lifetime = client.IdentityTokenLifetime;
            var expirationTime = creationTime.AddSeconds(lifetime);
            var token = new Token(
                code: id,
                type: TokenTypes.IdentityToken,
                lifetime: lifetime,
                claims: subject.Claims.ToArray(),
                expirationTime: expirationTime,
                creationTime: creationTime);
            return await _jwtTokens.CreateJwtTokenAsync(client, token);
        }
    }
}
