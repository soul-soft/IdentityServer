using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IProfileService _profileService;

        public ClaimsService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenRequest request)
        {
            var userClaims = request.Resources.UserClaims;
            var claims = FilterRequestedClaimTypes(userClaims);
            var profileDataRequest = new ProfileDataRequest(
                ProfileDataCaller.ClaimsProviderAccessToken,
                claims);
            return await _profileService.GetProfileDataAsync(profileDataRequest);
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenRequest request)
        {
            var userClaims = request.Resources.UserClaims;
            var claims = FilterRequestedClaimTypes(userClaims);
            var profileDataRequest = new ProfileDataRequest(
                ProfileDataCaller.ClaimsProviderIdentityToken,
                claims);
            return await _profileService.GetProfileDataAsync(profileDataRequest);
        }

        private IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claims)
        {
            foreach (var item in claims)
            {
                if (!Constants.ClaimsServiceFilterClaimTypes.Contains(item))
                {
                    yield return item;
                }
            }
        }
    }
}
