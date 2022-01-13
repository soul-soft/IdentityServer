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

        public async Task<GrantValidationResult> ValidateAsync(RefreshTokenGrantValidationContext context)
        {
            var refreshToken = await _refreshTokenService.FindRefreshTokenAsync(context.RefreshToken);
            if (refreshToken == null)
            {
                return GrantValidationResult.Error("Refresh token does not exist");
            }
            if (_clock.UtcNow.UtcDateTime > refreshToken.Expiration)
            {
                await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken);
                return GrantValidationResult.Error("Refresh token has expired");
            }
            if (refreshToken.AccessToken.ClientId != context.Request.Client.ClientId)
            {
                return GrantValidationResult.Error("The client ID of refreshtoken is wrong");
            }
            foreach (var item in context.Request.Scopes)
            {
                if (!refreshToken.AccessToken.Scopes.Contains(item))
                {
                    return GrantValidationResult.Error("Exceeded allowed scope");
                }
            }
            await _refreshTokenService.DeleteRefreshTokenAsync(refreshToken);
            return GrantValidationResult.Success();
        }
    }
}
