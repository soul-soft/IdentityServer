using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidator : IRefreshTokenRequestValidator
    {
        private readonly ISystemClock _clock;
        private readonly IRefreshTokenStore _refreshTokenStore;

        public RefreshTokenGrantValidator(
            ISystemClock clock,
            IRefreshTokenStore refreshTokenStore)
        {
            _clock = clock;
            _refreshTokenStore = refreshTokenStore;
        }

        public async Task ValidateAsync(RefreshTokenRequestValidation context)
        {
            var refreshToken = await _refreshTokenStore.FindRefreshTokenAsync(context.RefreshToken);
            if (refreshToken == null)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidGrant, "Invalid refresh token");
            }
            if (_clock.UtcNow.UtcDateTime > refreshToken.Expiration)
            {
                await _refreshTokenStore.RevomeRefreshTokenAsync(refreshToken);
                throw new ValidationException(OpenIdConnectErrors.InvalidGrant, "Refresh token has expired");
            }
            await _refreshTokenStore.RevomeRefreshTokenAsync(refreshToken);
        }
    }
}
