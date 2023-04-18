using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class RefreshTokenRequestValidator : IRefreshTokenRequestValidator
    {
        private readonly ITokenStore _tokenStore;

        public RefreshTokenRequestValidator(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public async Task<GrantValidationResult> ValidateAsync(RefreshTokenValidationRequest request)
        {
            var token = await _tokenStore.FindRefreshTokenAsync(request.RefreshToken);
            if (token == null)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            if (token.GetClientId() != request.Client.ClientId)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            await _tokenStore.RevomeTokenAsync(token);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(token.Claims, GrantTypes.RefreshToken));
            return new GrantValidationResult(subject);
        }
    }
}
