using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    internal class ClaimsService : IClaimsService
    {
        private readonly ISystemClock _clock;
        private readonly IProfileService _profileService;
        private readonly IClaimsValidator _claimsValidator;

        public ClaimsService(
            ISystemClock clock,
            IProfileService profileService,
            IClaimsValidator claimsValidator)
        {
            _clock = clock;
            _profileService = profileService;
            _claimsValidator = claimsValidator;
        }       

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var client = request.Client;
            var resources = request.Resources;
            var claims = new List<Claim>();
            if (resources.ClaimTypes.Contains(JwtClaimTypes.AuthenticationTime))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            }
            if (resources.ClaimTypes.Contains(JwtClaimTypes.IdentityProvider))
            {
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, request.Options.IdentityProvider));
            }
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.ClaimsProviderAccessToken, client, resources.ClaimTypes);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            claims.AddRange(profiles.ToClaims());
            //验证
            await _claimsValidator.ValidateAsync(claims, request.Resources.ClaimTypes);
            return claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var client = request.Client;
            var resources = request.Resources;
            var claims = new List<Claim>();
            if (resources.ClaimTypes.Contains(JwtClaimTypes.AuthenticationTime))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            }
            if (resources.ClaimTypes.Contains(JwtClaimTypes.IdentityProvider))
            {
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, request.Options.IdentityProvider));
            }
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.ClaimsProviderAccessToken, client, resources.ClaimTypes);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            claims.AddRange(profiles.ToClaims());
            return claims;
        }
    }
}
