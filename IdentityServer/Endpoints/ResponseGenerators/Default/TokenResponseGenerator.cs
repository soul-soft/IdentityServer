namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;
        private readonly IReferenceTokenStore _refreshTokenStore;

        public TokenResponseGenerator(
            ITokenService tokenService,
            IReferenceTokenStore refreshTokenStore)
        {
            _tokenService = tokenService;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task<TokenResponse> ProcessAsync(ValidatedTokenRequest request)
        {
            (string accessToken, string? refreshToken) = await CreateAccessTokenAsync(request);
            var scope = string.Join(",", request.Resources.Scopes);
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
                var refreshToken = await _refreshTokenStore.StoreReferenceTokenAsync(token);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
