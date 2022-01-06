using System.Security.Claims;

namespace IdentityServer.Application
{
    public class ClaimsService : IClaimsService
    {
        private readonly IProfileService _profile;
     
        public ClaimsService(IProfileService profile)
        {
            _profile = profile;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ClaimsRequest request)
        {
            var claimTypes = FilterRequestedClaimTypes(request.RequestedClaimTypes);
            return await _profile.GetProfileDataAsync(new ProfileDataRequestRequest(
                ProfileDataRequestCaller.ClaimsProviderAccessToken,
                request.Client,
                request.RequestedClaimTypes
                ));
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ClaimsRequest request)
        {
            var claimTypes = FilterRequestedClaimTypes(request.RequestedClaimTypes);
            return await _profile.GetProfileDataAsync(new ProfileDataRequestRequest(
                 ProfileDataRequestCaller.ClaimsProviderIdentityToken,
                 request.Client,
                 claimTypes
                 ));
        }

        protected virtual IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claimTypes)
        {
            var claimTypesToFilter = claimTypes
                .Where(x => Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(x));
            return claimTypes.Except(claimTypesToFilter);
        }
    }
}
