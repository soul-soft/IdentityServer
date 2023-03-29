namespace IdentityServer.Validation
{
    internal class RefreshTokenRequestValidator : IRefreshTokenRequestValidator
    {
        private readonly ITokenStore _tokenStore;

        public RefreshTokenRequestValidator(ITokenStore tokenStore)
        {
            _tokenStore = tokenStore;
        }

        public async Task<RefreshTokenValidationResult> ValidateAsync(RefreshTokenValidationRequest request)
        {
            var token = await _tokenStore.FindTokenAsync(request.RefreshToken);
            if (token == null)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            if (token.GetClientId() != request.Client.ClientId)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            await _tokenStore.RevomeTokenAsync(token);
            return new RefreshTokenValidationResult(token.Claims);
        }
    }
}
