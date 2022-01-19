using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ClaimsService : IClaimsService
    {
        private readonly ISystemClock _clock;
        private readonly IProfileService _profileService;
        private readonly IdentityServerOptions _options;

        public ClaimsService(
            ISystemClock clock,
            IProfileService profileService,
            IdentityServerOptions options)
        {
            _clock = clock;
            _options = options;
            _profileService = profileService;
        }

        public async Task<ClaimsPrincipal> CreateSubjectAsync(GrantRequest request, GrantValidationResult result)
        {
            var resources = request.Resources;
            var identity = new ClaimsIdentity(request.GrantType);
            identity.AddClaims(result.Claims);
            if (resources.UserClaims.Contains(JwtClaimTypes.AuthenticationTime))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            }
            if (resources.UserClaims.Contains(JwtClaimTypes.IdentityProvider))
            {
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityServerName));
            }
            if (resources.UserClaims.Contains(JwtClaimTypes.Subject))
            {
                if (!string.IsNullOrEmpty(result.Subject) && !identity.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
                {
                    identity.AddClaim(new Claim(JwtClaimTypes.Subject, result.Subject));
                }
            }
            var claims = await _profileService.GetUserClaimsAsync(new UserClaimsProfileRequest(
                request.Client,
                request.Resources,
                request.GrantType));
            identity.AddClaims(claims);
            return new ClaimsPrincipal(identity);
        }
    }
}
