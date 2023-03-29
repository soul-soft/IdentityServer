using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IIdGenerator _idGenerator;
        private readonly IdentityServerOptions _options;
        private readonly IProfileService _profileService;

        public ClaimService(
            IServerUrl serverUrl,
            IIdGenerator idGenerator,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IProfileService profileService)
        {
            _options = options;
            _idGenerator = idGenerator;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _profileService = profileService;
        }

        public async Task<ClaimsPrincipal> GetAccessTokenClaimsAsync(string grantType, ProfileClaimsRequest request)
        {
            #region Jwt Claims
            //request jwt
            var jwtId = await _idGenerator.GenerateAsync(16);
            var issuer = _serverUrl.GetIdentityServerIssuerUri();
            var issuedAt = _systemClock.UtcNow.ToUnixTimeSeconds();
            var expiration = issuedAt + request.Client.AccessTokenLifetime;
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.JwtId, jwtId),
                new Claim(JwtClaimTypes.Issuer, issuer),
                new Claim(JwtClaimTypes.ClientId, request.Client.ClientId),
                new Claim(JwtClaimTypes.IssuedAt, issuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.NotBefore, issuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.Expiration, expiration.ToString(), ClaimValueTypes.Integer64)
            };

            //audience
            foreach (var item in request.Resources.ApiResources)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item.Name));
            }
            //scope
            if (_options.EmitScopesAsCommaDelimitedStringInJwt)
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

            if (request.Subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                claims.AddRange(GetStandardSubjectClaims(request.Subject, request.Resources.ClaimTypes));
            }
            #endregion

            #region Subject Claims
            claims.AddRange(FilterRequestClaims(request.Subject.Claims, request.Resources.ClaimTypes));
            #endregion

            #region Profile Cliams
            var profileDataClaims = await _profileService.GetAccessTokenClaimsAsync(new ProfileClaimsRequest(request.Subject, request.Client, request.Resources));
            claims.AddRange(FilterRequestClaims(profileDataClaims, request.Resources.ClaimTypes));
            #endregion

            return new ClaimsPrincipal(new ClaimsIdentity(claims, grantType));
        }

        public async Task<ClaimsPrincipal> GetProfileClaimsAsync(ProfileClaimsRequest context)
        {
            var claims = new List<Claim>();

            var profileDataClaims = await _profileService.GetProfileClaimsAsync(new ProfileClaimsRequest(context.Subject, context.Client, context.Resources));
            claims.AddRange(FilterRequestClaims(profileDataClaims, context.Resources.ClaimTypes));

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
        }

        private IEnumerable<Claim> FilterRequestClaims(IEnumerable<Claim> claims, IEnumerable<string> claimTypes)
        {
            if (_options.EnableClaimTypeFilter)
            {
                return claims.Where(a => claimTypes.Contains(a.Type))
                    .Where(a => !internalConstants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
            }
            return claims;
        }

        private IEnumerable<Claim> GetStandardSubjectClaims(ClaimsPrincipal subject, IEnumerable<string> claimTypes)
        {
            if (_options.EnableClaimTypeFilter && !claimTypes.Contains(JwtClaimTypes.Subject))
            {
                yield break;
            }
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.Subject).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.IdentityProvider).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationTime).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationMethod).First();
        }
    }
}
