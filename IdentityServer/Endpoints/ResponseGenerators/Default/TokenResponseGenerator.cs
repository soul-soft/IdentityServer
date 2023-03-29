﻿namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenGeneratorResponse> ProcessAsync(TokenGeneratorRequest request)
        {
            (string accessToken, string? refreshToken) = await CreateTokenAsync(request);
            var scope = string.Join(",", request.Resources.Scopes);
            var tokenLifetime = request.Client.AccessTokenLifetime;
            var response = new TokenGeneratorResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenLifetime = tokenLifetime,
                Scope = scope,
            };
            return response;
        }

        private async Task<(string accessToken, string? refreshToken)> CreateTokenAsync(TokenGeneratorRequest request)
        {
            var accessToken = await _tokenService.CreateAccessTokenAsync(
                request.Client.AccessTokenType,
                request.Client.AccessTokenLifetime,
                request.Client.AllowedSigningAlgorithms,
                request.Subject.Claims);

            if (request.Resources.OfflineAccess)
            {
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(accessToken, request.Client.RefreshTokenLifetime);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
