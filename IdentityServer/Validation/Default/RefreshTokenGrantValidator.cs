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
            var refreshToken = await _refreshTokenService.GetAsync(context.RefreshToken);
            if (refreshToken == null)
            {
                throw new InvalidGrantException("Refresh token does not exist");
            }
            if (_clock.UtcNow.UtcDateTime > refreshToken.Expiration)
            {
                await _refreshTokenService.DeleteAsync(refreshToken);
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
                    throw new InvalidGrantException("Exceeded allowed scope");
                }
            }
            await _refreshTokenService.DeleteAsync(refreshToken);
            return new GrantValidationResult();
        }
    }
}
