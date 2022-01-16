using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
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



        public ClaimsService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var list = new List<Claim>();
            var profileDataRequest = new ProfileDataRequestContext(
                request.Client,
                request.Subject,
                ProfileDataCaller.ClaimsProviderIdentityToken,
                FilterRequestedClaimTypes(request.Resources.UserClaims));
            await _profileService.GetProfileDataAsync(profileDataRequest);
            list.AddRange(profileDataRequest.IssuedClaims);
            var standardClaims = GetStandardClaims(request.Subject);
            list.AddRange(standardClaims);
            return list;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var list = new List<Claim>();
            var profileDataRequest = new ProfileDataRequestContext(
                request.Client,
                request.Subject,
                ProfileDataCaller.ClaimsProviderIdentityToken,
                FilterRequestedClaimTypes(request.Resources.UserClaims));
            await _profileService.GetProfileDataAsync(profileDataRequest);
            list.AddRange(profileDataRequest.IssuedClaims);
            var standardClaims = GetStandardClaims(request.Subject);
            list.AddRange(standardClaims);
            return list;
        }

        protected virtual IEnumerable<Claim> GetStandardClaims(ClaimsPrincipal subject)
        {
            var claims = subject.Claims
                .Where(a =>
                       a.Type == JwtClaimTypes.Subject
                    || a.Type == JwtClaimTypes.IdentityProvider
                    || a.Type == JwtClaimTypes.AuthenticationTime).ToList();
            if (!string.IsNullOrWhiteSpace(subject.Identity?.AuthenticationType))
            {
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, subject.Identity.AuthenticationType));
            }
            return claims;
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
