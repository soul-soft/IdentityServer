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
            var identity = await GetStandardClaimsAsync(request);
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
            return identity.Claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var identity = await GetStandardClaimsAsync(request);
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
            return identity.Claims;
        }

        public async Task<ClaimsIdentity> GetStandardClaimsAsync(ValidatedTokenRequest request)
        {
            var allowClaimTypes = request.Resources.ClaimTypes;
            var claimDataRequestContext = new ClaimDataRequestContext(
               ClaimsProviders.AccessToken,
               request.Client,
               allowClaimTypes);
            var identity = new ClaimsIdentity();
            var claims = await _profileService.GetClaimDataAsync(claimDataRequestContext);
            identity.AddClaims(claims);
            if (identity.HasClaim(a => a.Type == JwtClaimTypes.Subject))
            {
                var timestamp = _clock.UtcNow.ToUnixTimeSeconds().ToString();
                identity.AddClaim(new Claim(JwtClaimTypes.IdentityProvider, request.Options.IdentityProvider));
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationTime, timestamp));
                identity.AddClaim(new Claim(JwtClaimTypes.AuthenticationMethod, request.GrantType));
            }
            return identity;
        }
    }
}
