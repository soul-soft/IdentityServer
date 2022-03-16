namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ProcessAsync(TokenRequest request)
        {
            (string accessToken, string? refreshToken) = await CreateAccessTokenAsync(request);
            var scope = string.Join(",", request.Resources.Scopes);
            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                TokenLifetime = request.Client.AccessTokenLifetime,
                Scope = scope,
            };
            return response;
        }

        private async Task<(string accessToken, string? refreshToken)> CreateAccessTokenAsync(TokenRequest request)
        {
            var token = await _tokenService.CreateAccessTokenAsync(request);

            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);

            if (request.Resources.OfflineAccess)
            {
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(token, request.Client.RefreshTokenLifetime);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
