using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IProfileService _profileService;

        public ClaimsService(IProfileService profileService)
        {
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetAllowedClaimsAsync(request);
            return claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = await GetAllowedClaimsAsync(request);
            return claims;
        }

        private async Task<List<Claim>> GetAllowedClaimsAsync(ValidatedTokenRequest request)
        {
            var claims = new List<Claim>();

            //clientId
            claims.Add(new Claim(JwtClaimTypes.ClientId, request.Client.ClientId));

            //issuer
            claims.Add(new Claim(JwtClaimTypes.Issuer, request.Options.Issuer));

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
            //request claims
            var claimTypes = request.Resources.ClaimTypes;
            var claimDataRequestContext = new ClaimDataRequestContext(
                ClaimsProviders.AccessToken,
                request.Client,
                claimTypes);
            var allowClaims = await _profileService.GetClaimDataAsync(claimDataRequestContext);
            foreach (var item in allowClaims)
            {
                if (!claimTypes.Any(a => a == item.Type))
                {
                    continue;
                }
                if (claims.Any(a => a.Type == item.Type))
                {
                    continue;
                }
                claims.Add(item);
            }
            return claims;
        }
    }
}
