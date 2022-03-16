using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IProfileService _profileService;
        private readonly IHandleGenerator _handleGenerator;

        public AuthenticationService(
            IServerUrl serverUrl,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IProfileService profileService,
            IHandleGenerator handleGenerator)
        {
            _options = options;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _profileService = profileService;
            _handleGenerator = handleGenerator;
        }

        public async Task<ClaimsPrincipal> SingInAsync(string grantType, Client client, Resources resources)
        {
            var jwtId = await _handleGenerator.GenerateAsync();
            var issuedAt = _systemClock.UtcNow.ToUnixTimeSeconds();
            var jwtClaims = GetStandardJwtClaims(jwtId, issuedAt, client, resources);
            var claims = new List<Claim>(jwtClaims);
            //request claims
            var claimTypes = resources.ClaimTypes;
            var profileDataRequest = new ProfileDataRequestContext(
                ProfileDataCallers.AccessToken,
                client,
                resources,
                claimTypes);
            var requestedClaims = await _profileService.GetProfileDataAsync(profileDataRequest);
            //standard claims
            if (requestedClaims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                var subject = requestedClaims
                    .Where(a => a.Type == JwtClaimTypes.Subject)
                    .First().Value;
                var standardClaims = GetSubjectStandardClaims(subject, grantType, issuedAt);
                claims.AddRange(standardClaims);
            }
            var allowedRequestClaims = requestedClaims
                .Where(a => claimTypes.Contains(a.Type))
                .Where(a => !Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
            claims.AddRange(allowedRequestClaims);
            return new ClaimsPrincipal(new ClaimsIdentity(claims, "TokenRequestEndpoint"));
        }


        private IEnumerable<Claim> GetStandardJwtClaims(string jwtId, long issuedAt, Client client, Resources resources)
        {
            var claims = new List<Claim>();
            //clientId
            claims.Add(new Claim(JwtClaimTypes.ClientId, client.ClientId));

            //issuer
            claims.Add(new Claim(JwtClaimTypes.Issuer, _serverUrl.GetIdentityServerIssuerUri()));

            //audience
            foreach (var item in resources.ApiResources)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item.Name));
            }

            //scope
            if (_options.EmitScopesAsCommaDelimitedStringInJwt)
            {
                var scope = resources.Scopes.Aggregate((x, y) => $"{x},{y}");
                claims.Add(new Claim(JwtClaimTypes.Scope, scope));
            }
            else
            {
                var scopes = resources.Scopes
                    .Select(scope => new Claim(JwtClaimTypes.Scope, scope));
                claims.AddRange(scopes);
            }

            var expiration = issuedAt + client.AccessTokenLifetime;
            claims.Add(new Claim(JwtClaimTypes.JwtId, jwtId));
            claims.Add(new Claim(JwtClaimTypes.NotBefore, issuedAt.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, issuedAt.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.Expiration, expiration.ToString(), ClaimValueTypes.Integer64));
            return claims;
        }

        private IEnumerable<Claim> GetSubjectStandardClaims(string subject, string authenticationMethod, long authenticationTime)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(JwtClaimTypes.Subject, subject));
            claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, authenticationTime.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityProvider));
            claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, authenticationMethod));
            return claims;
        }
    }
}
