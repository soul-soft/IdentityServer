using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimIssueService : IClaimsService
    {
        private readonly IProfileService _profileService;

        private static string[] _filterClaimTypes =
        {
            JwtClaimTypes.AccessTokenHash,
            JwtClaimTypes.Audience,
            JwtClaimTypes.AuthenticationMethod,
            JwtClaimTypes.AuthenticationTime,
            JwtClaimTypes.AuthorizedParty,
            JwtClaimTypes.IdentityProvider,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.ClientId,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.Issuer,
            JwtClaimTypes.JwtId,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.SessionId,
            JwtClaimTypes.IdentityProvider,
            JwtClaimTypes.Subject,
            JwtClaimTypes.Scope,
            JwtClaimTypes.Confirmation
        };

        public ClaimIssueService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenRequest request)
        {
            var list = new List<Claim>();
            var claimTypes = FilterRequestedClaimTypes(request.Resources.UserClaims);
            var profileDataRequest = new ProfileDataRequestContext(
                request.Client,
                request.Subject,
                ProfileDataCaller.ClaimsProviderAccessToken,
                claimTypes);
            await _profileService.GetProfileDataAsync(profileDataRequest);
            list.AddRange(profileDataRequest.IssuedClaims);
            return FilterProtocolClaims(list);
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenRequest request)
        {
            var list = new List<Claim>();
            var claimTypes = FilterRequestedClaimTypes(request.Resources.UserClaims);
            var profileDataRequest = new ProfileDataRequestContext(
                request.Client,
                request.Subject,
                ProfileDataCaller.ClaimsProviderIdentityToken,
                claimTypes);
            await _profileService.GetProfileDataAsync(profileDataRequest);
            list.AddRange(profileDataRequest.IssuedClaims);
            return FilterProtocolClaims(list);
        }

        private IEnumerable<Claim> FilterProtocolClaims(IEnumerable<Claim> claims)
        {
            foreach (var item in claims)
            {
                if (!_filterClaimTypes.Any(a => a == item.Type))
                {
                    yield return item;
                }
            }
        }

        private IEnumerable<string> FilterRequestedClaimTypes(IEnumerable<string> claims)
        {
            foreach (var item in claims)
            {
                if (!_filterClaimTypes.Contains(item))
                {
                    yield return item;
                }
            }
        }
    }
}
