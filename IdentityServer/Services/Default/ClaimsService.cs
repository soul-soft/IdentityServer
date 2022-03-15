using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IServerUrl _serverUrl;
        private readonly IProfileService _profileService;

        public ClaimsService(
            IServerUrl serverUrl,
            IProfileService profileService)
        {
            _serverUrl = serverUrl;
            _profileService = profileService;
        }

        public async Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(TokenValidatedRequest request)
        {
            var claims = await GetAllowedClaimsAsync(request);
            return claims;
        }

        public async Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(TokenValidatedRequest request)
        {
            var claims = await GetAllowedClaimsAsync(request);
            return claims;
        }

        private async Task<List<Claim>> GetAllowedClaimsAsync(TokenValidatedRequest request)
        {
            var claims = new List<Claim>();

            //clientId
            claims.Add(new Claim(JwtClaimTypes.ClientId, request.Client.ClientId));

            //issuer
            claims.Add(new Claim(JwtClaimTypes.Issuer, _serverUrl.GetIdentityServerIssuerUri()));

            //audience
            foreach (var item in request.Resources.ApiResources)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item.Name));
            }

            //scope
            if (request.Options.EmitScopesAsCommaDelimitedStringInJwt)
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
            var requestedClaims = await _profileService.GetProfileDataAsync(new ProfileDataRequestContext(
                ClaimsProviders.AccessToken,
                request.Client,
                request.Resources,
                claimTypes));
            foreach (var item in requestedClaims)
            {
                if (!claimTypes.Any(a => a == item.Type))
                {
                    continue;
                }
                if (Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(item.Type))
                {
                    continue;
                }
                claims.Add(item);
            }
            return claims;
        }
    }
}
