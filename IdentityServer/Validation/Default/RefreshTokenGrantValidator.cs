using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidator : IRefreshTokenGrantValidator
    {
        private readonly IRefreshTokenService _refreshTokenService;

        private readonly ISystemClock _clock;

        public RefreshTokenGrantValidator(
            ISystemClock clock,
            IRefreshTokenService refreshTokenService)
        {
            _clock = clock;
            _refreshTokenService = refreshTokenService;
        }

        public async Task ValidateAsync(RefreshTokenGrantValidationRequest context)
        {
            var refreshToken = await _refreshTokenService.GetRefreshTokenAsync(context.RefreshToken);
            if (refreshToken == null)
            {
                throw new InvalidGrantException("Invalid refresh token");
            }
            if (_clock.UtcNow.UtcDateTime > refreshToken.Expiration)
            {
                await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken);
                throw new InvalidGrantException("Refresh token has expired");
            }
            if (refreshToken.AccessToken.ClientId != context.Request.Client.ClientId)
            {
                throw new InvalidGrantException("The client ID of refreshtoken is change");
            }
            foreach (var item in context.Request.Scopes)
            {
                if (!refreshToken.AccessToken.Scopes.Contains(item))
                {
                    throw new InvalidGrantException("Unable to expand scope");
                }
            }
            await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken);
        }
    }
}
