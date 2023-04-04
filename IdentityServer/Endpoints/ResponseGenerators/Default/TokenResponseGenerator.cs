using IdentityServer.Models;

namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(
            ITokenService tokenService)
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
                IdentityToken = accessToken,
                TokenLifetime = tokenLifetime,
                Scope = scope,
            };
            return response;
        }

        private async Task<(string accessToken, string? refreshToken)> CreateTokenAsync(TokenGeneratorRequest request)
        {           
            var accessToken = await _tokenService.CreateAccessTokenAsync(request.Client, request.Subject);

            if (request.Client.OfflineAccess)
            {
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(request.Client, request.Subject);
                return (accessToken, refreshToken);
            }

            return (accessToken, null);
        }
    }
}
