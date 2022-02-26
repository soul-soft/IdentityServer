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

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(Client client,ResourceCollection resources)
        {
            var claims = new List<Claim>();
            if (resources.ClaimTypes.Contains(JwtClaimTypes.AuthenticationTime))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            }
            if (resources.ClaimTypes.Contains(JwtClaimTypes.IdentityProvider))
            {
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityProvider));
            }
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.ClaimsProviderAccessToken, client, resources.ClaimTypes);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            claims.AddRange(profiles.ToClaims());
            return claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(Client client, ResourceCollection resources)
        {
            var claims = new List<Claim>();
            if (resources.ClaimTypes.Contains(JwtClaimTypes.AuthenticationTime))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            }
            if (resources.ClaimTypes.Contains(JwtClaimTypes.IdentityProvider))
            {
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityProvider));
            }
            var profileDataRequestContext = new ProfileDataRequestContext(ProfileDataCallers.ClaimsProviderIdentityToken, client, resources.ClaimTypes);
            var profiles = await _profileService.GetProfileDataAsync(profileDataRequestContext);
            claims.AddRange(profiles.ToClaims());
            return claims;
        }
    }
}
