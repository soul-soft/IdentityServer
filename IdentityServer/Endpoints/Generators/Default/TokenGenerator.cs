namespace IdentityServer.Endpoints
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ITokenService _tokenService;

        private readonly IRefreshTokenService _refreshTokenService;

        public TokenGenerator(
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<TokenResponse> ProcessAsync(ValidatedTokenRequest request)
        {
            (string accessToken, string? refreshToken) = await CreateAccessTokenAsync(request);

            var scope = string.Join(",", request.Scopes);

            var response = new TokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenLifetime = request.Client.AccessTokenLifetime,
                Scope = scope,
            };

            return response;
        }

        private async Task<(string accessToken, string? refreshToken)> CreateAccessTokenAsync(ValidatedTokenRequest request)
        {
            var token = await _tokenService.CreateAccessTokenAsync(request);
           
            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);
          
            if (request.Resources.OfflineAccess)
            {
                var refreshToken = await _refreshTokenService
                    .CreateAsync(token, request.Client.RefreshTokenLifetime);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
