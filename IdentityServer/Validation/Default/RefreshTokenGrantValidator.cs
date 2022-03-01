using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    public class RefreshTokenGrantValidator : IRefreshTokenGrantValidator
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

        public async Task ValidateAsync(RefreshTokenGrantValidationRequest context)
        {
            var refreshToken = await _refreshTokenStore.FindRefreshTokenAsync(context.RefreshToken);
            if (refreshToken == null)
            {
                throw new InvalidGrantException("Invalid refresh token");
            }
            if (_clock.UtcNow.UtcDateTime > refreshToken.Expiration)
            {
                await _refreshTokenStore.RevomeRefreshTokenAsync(refreshToken);
                throw new InvalidGrantException("Refresh token has expired");
            }
            //if (refreshToken.AccessToken.ClientId != context.Request.Client.ClientId)
            //{
            //    throw new InvalidGrantException("The client ID of refreshtoken is change");
            //}
            //foreach (var item in context.Request.Scopes)
            //{
            //    if (!refreshToken.AccessToken.Scopes.Contains(item))
            //    {
            //        throw new InvalidGrantException("Unable to expand scope");
            //    }
            //}
            await _refreshTokenStore.RevomeRefreshTokenAsync(refreshToken);
        }
    }
}
