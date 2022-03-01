using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly ISystemClock _clock;
        private readonly IProfileService _profileService;

        public ClaimsService(
            ISystemClock clock,
            IProfileService profileService)
        {
            _clock = clock;
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetClaimDataAsync(request);
            var identity = new ClaimsIdentity(claims, request.GrantType);
            identity.AddClaim(new Claim(JwtClaimTypes.ClientId, request.Client.ClientId));
            identity.AddClaim(new Claim(JwtClaimTypes.Issuer, request.Options.Issuer));
            //audience
            foreach (var item in request.Resources.ApiResources)
            {
                identity.AddClaim(new Claim(JwtClaimTypes.Audience, item.Name));
            }
            //scope
            if (request.Options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                var scope = request.Resources.Scopes.Aggregate((x, y) => $"{x},{y}");
                identity.AddClaim(new Claim(JwtClaimTypes.Scope, scope));
            }
            else
            {
                var scopes = request.Resources.Scopes
                    .Select(scope => new Claim(JwtClaimTypes.Scope, scope));
                identity.AddClaims(scopes);
            }
            if (identity.HasClaim(a => a.Type == JwtClaimTypes.Subject))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, request.Options.IdentityProvider));
            }
            return identity.Claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetClaimDataAsync(request);
            var identity = new ClaimsIdentity(claims, request.GrantType);
            if (identity.HasClaim(a => a.Type == JwtClaimTypes.Subject))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, request.Options.IdentityProvider));
            }
            return identity.Claims;
        }

        public async Task<IEnumerable<Claim>> GetClaimDataAsync(ValidatedTokenRequest request)
        {
            var allowClaims = request.Resources.ClaimTypes;
            var claimDataRequestContext = new ClaimDataRequestContext(
               ClaimsProviders.AccessToken,
               request.Client,
               allowClaims);
            var claims = await _profileService.GetClaimDataAsync(claimDataRequestContext);
            return claims.Where(a => allowClaims.Contains(a.Type));
        }
    }
}
