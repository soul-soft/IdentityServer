using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly ISystemClock _clock;
        private readonly IProfileService _profileService;

        public ClaimsService(ISystemClock clock, IProfileService profileService)
        {
            _clock = clock;
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetRequestedClaimsAsync(request);
            return claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetRequestedClaimsAsync(request);
            return claims;
        }

        private async Task<List<Claim>> GetRequestedClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = new List<Claim>();

            //profile
            var allowClaimTypes = request.Resources.ClaimTypes;
            var issueClaims = await _profileService.GetClaimDataAsync(new ClaimDataRequestContext(
                ClaimsProviders.AccessToken,
                request.Client,
                allowClaimTypes));
            //filter
            claims.AddRange(issueClaims.Where(a => allowClaimTypes.Contains(a.Type)));
            //issuer
            claims.Add(new Claim(JwtClaimTypes.Issuer, request.Options.Issuer));
            //clientId
            claims.Add(new Claim(JwtClaimTypes.ClientId, request.Client.ClientId));
            //audience
            foreach (var item in request.Resources.ApiResources)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item.Name));
            }
            //scope
            if (request.Options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                var scope = request.Resources.Scopes.Aggregate((x, y) => $"{x},{y}");
                claims.Add(new Claim(JwtClaimTypes.Scope, scope));
            }
            else
            {
                var scopes = request.Resources.Scopes
                    .Select(scope => new Claim(JwtClaimTypes.Scope, scope));
                claims.AddRange(scopes);
            }

            if (claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                claims.AddRange(GetStandardSubjectClaims(request));
            }
            return claims;
        }

        private IEnumerable<Claim> GetStandardSubjectClaims(ValidatedTokenRequest request)
        {
            var claims = new List<Claim>();
            var identityProvider = request.Options.IdentityProvider;
            var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
            claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, identityProvider));
            claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, request.GrantType));
            return claims;
        }
    }
}
