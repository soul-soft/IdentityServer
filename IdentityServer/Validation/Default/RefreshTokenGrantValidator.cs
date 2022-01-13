namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidator : IRefreshTokenGrantValidator
    {
        private readonly IRefreshTokenService _refreshTokenService;

        public RefreshTokenGrantValidator(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<GrantValidationResult> ValidateAsync(RefreshTokenGrantValidationContext context)
        {
            var token = await _refreshTokenService.FindRefreshTokenAsync(context.RefreshToken);
            if (token == null)
            {
                return GrantValidationResult.Error("Invalid refreshToken");
            }
            return GrantValidationResult.Success();
        }
    }
}
