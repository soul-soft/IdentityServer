using IdentityServer.Models;
using System.Resources;

namespace IdentityServer.Endpoints
{
    public class TokenResponseGenerator : ITokenResponseGenerator
    {
        private readonly ITokenService _tokenService;
        private readonly IClaimService _claimService;

        public TokenResponseGenerator(
            ITokenService tokenService,
            IClaimService claimService)
        {
            _tokenService = tokenService;
            _claimService = claimService;
        }

        public async Task<TokenGeneratorResponse> CreateTokenAsync(TokenGeneratorRequest request)
        {
            string? refreshToken = null;
            string? identityToken = null;
            var scope = string.Join(",", request.Resources.Scopes);
            var tokenLifetime = request.Client.AccessTokenLifetime;
            string accessToken = await CreateAccessTokenAsync(request);
            if (request.Client.OfflineAccess)
            {
                refreshToken = await CreateRefreshTokenAsync(request);
            }
            if (request.GrantType == GrantTypes.AuthorizationCode)
            {
                refreshToken = await CreateIdentityTokenAsync(request);
            }
            var response = new TokenGeneratorResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                IdentityToken = identityToken,
                TokenLifetime = tokenLifetime,
                Scope = scope,
            };
            return response;
        }

        private async Task<string> CreateAccessTokenAsync(TokenGeneratorRequest request)
        {
            var subject = await _claimService.GetTokenClaimsAsync(request);
            return await _tokenService.CreateAccessTokenAsync(request.Client, subject);
        }

        private async Task<string> CreateIdentityTokenAsync(TokenGeneratorRequest request)
        {
            var subject = await _claimService.GetTokenClaimsAsync(request);
            return await _tokenService.CreateIdentityTokenAsync(request.Client, subject, request.Code!);
        }

        private async Task<string> CreateRefreshTokenAsync(TokenGeneratorRequest request)
        {
            var subject = await _claimService.GetTokenClaimsAsync(request);
            return await _tokenService.CreateRefreshTokenAsync(request.Client, subject);
        }
    }
}
