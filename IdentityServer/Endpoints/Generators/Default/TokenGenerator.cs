namespace IdentityServer.Endpoints
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly ITokenService _tokenService;
        private readonly IClaimsService _claimsService;
        private readonly IClaimsValidator _claimsValidator;
        private readonly IRefreshTokenService _refreshTokenService;

        public TokenGenerator(
            ITokenService tokenService,
            IClaimsService claimsService,
            IClaimsValidator claimsValidator,
            IRefreshTokenService refreshTokenService)
        {
            _tokenService = tokenService;
            _claimsService = claimsService;
            _claimsValidator = claimsValidator;
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
            var claims = await _claimsService.GetAccessTokenClaimsAsync(request.Client, request.Resources);
            await _claimsValidator.ValidateAsync(claims, request.Resources.ClaimTypes);

            var token = await _tokenService.CreateAccessTokenAsync(request);

            var accessToken = await _tokenService.CreateSecurityTokenAsync(token);

            if (request.Resources.OfflineAccess)
            {
                var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(token, request.Client.RefreshTokenLifetime);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
