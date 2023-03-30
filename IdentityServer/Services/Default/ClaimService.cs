using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IProfileService _profileService;

        public ClaimService(
            IServerUrl serverUrl,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IProfileService profileService,
            IRandomGenerator randomGenerator)
        {
            _options = options;
            _randomGenerator = randomGenerator;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _profileService = profileService;
        }

        public async Task<ClaimsPrincipal> GetAccessTokenClaimsAsync(string grantType, ProfileClaimsRequest request)
        {
            #region Jwt Claims
            //request jwt
            var jwtId = await _randomGenerator.GenerateAsync(16);
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
                claims.AddRange(GetStandardSubjectClaims(grantType, request.Subject, request.Resources.ClaimTypes));
            }
            #endregion

            #region Subject Claims
            claims.AddRange(FilterRequestClaims(request.Subject.Claims, request.Resources.ClaimTypes));
            #endregion

            #region Profile Cliams
            var profileDataClaims = await _profileService.GetAccessTokenClaimsAsync(new ProfileClaimsRequest(request.Subject, request.Client, request.Resources));
            claims.AddRange(FilterRequestClaims(profileDataClaims, request.Resources.ClaimTypes));
            #endregion

            return new ClaimsPrincipal(new ClaimsIdentity(claims));
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
            return claims.Where(a => claimTypes.Contains(a.Type))
                .Where(a => !OpenIdConnectConstants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
        }

        private IEnumerable<Claim> GetStandardSubjectClaims(string grantType, ClaimsPrincipal subject, IEnumerable<string> claimTypes)
        {
            if (claimTypes.Contains(JwtClaimTypes.Subject) && subject.GetSubjectId() != null)
            {
                yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.Subject).First();
                yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationMethod).FirstOrDefault()
                    ?? new Claim(JwtClaimTypes.AuthenticationMethod, grantType);

                yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.IdentityProvider).FirstOrDefault()
                    ?? new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityProvider);

                yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationTime).FirstOrDefault()
                    ?? new Claim(JwtClaimTypes.AuthenticationTime, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64);

            }
        }
    }
}
