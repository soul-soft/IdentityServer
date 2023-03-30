using IdentityServer.Models;

namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly IClaimService _claimService;
        private readonly ITokenService _tokenService;

        public TokenResponseGenerator(
            IClaimService claimService,
            ITokenService tokenService)
        {
            _claimService = claimService;
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
            var subject = await _claimService.GetAccessTokenClaimsAsync(request.GrantType, new ProfileClaimsRequest(request.Subject, request.Client, request.Resources));

            var accessToken = await _tokenService.CreateAccessTokenAsync(request.Client, subject);

            if (request.Client.OfflineAccess)
            {
                var refreshToken = await _tokenService.CreateRefreshTokenAsync(request.Client, request.Subject);
                return (accessToken, refreshToken);
            }
            return (accessToken, null);
        }
    }
}
