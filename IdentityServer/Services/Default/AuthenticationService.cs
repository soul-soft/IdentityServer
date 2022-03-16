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

        public async Task<ClaimsPrincipal> SingInAsync(AuthenticationSingInContext context)
        {
            //request jwt
            var jwtId = await _handleGenerator.GenerateAsync();
            var issuedAt = _systemClock.UtcNow.ToUnixTimeSeconds();

            var jwtClaims = GetAccessTokenClaims(jwtId, issuedAt, context);
            var claims = new List<Claim>(jwtClaims);

            #region Profile Claims
            var profileClaimTypes = context.Resources.ClaimTypes;
            var profileDataRequest = new ProfileDataRequestContext(
                ProfileDataCallers.TokenEndpoint,
                context.Client,
                context.Resources,
                profileClaimTypes);
            var requestedClaims = await _profileService.GetProfileDataAsync(profileDataRequest);
            var allowedRequestClaims = requestedClaims
                .Where(a => profileClaimTypes.Contains(a.Type))
                .Where(a => !Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
            claims.AddRange(allowedRequestClaims);
            #endregion

            #region Standard Claims
            if (requestedClaims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                var subject = requestedClaims
                    .Where(a => a.Type == JwtClaimTypes.Subject)
                    .First().Value;
                var identityProvider = _options.IdentityProvider;
                var standardClaims = GetSubjectStandardClaims(subject, issuedAt, context.GrantType, identityProvider);
                claims.AddRange(standardClaims);
            }
            #endregion

            return new ClaimsPrincipal(new ClaimsIdentity(claims, context.GrantType));
        }


        private IEnumerable<Claim> GetAccessTokenClaims(string jwtId, long issuedAt, AuthenticationSingInContext context)
        {
            var claims = new List<Claim>
            {
                //clientId
                new Claim(JwtClaimTypes.ClientId, context.Client.ClientId),

                //issuer
                new Claim(JwtClaimTypes.Issuer, _serverUrl.GetIdentityServerIssuerUri())
            };

            //audience
            foreach (var item in context.Resources.ApiResources)
            {
                claims.Add(new Claim(JwtClaimTypes.Audience, item.Name));
            }

            //scope
            if (_options.EmitScopesAsCommaDelimitedStringInJwt)
            {
                var scope = context.Resources.Scopes.Aggregate((x, y) => $"{x},{y}");
                claims.Add(new Claim(JwtClaimTypes.Scope, scope));
            }
            else
            {
                var scopes = context.Resources.Scopes
                    .Select(scope => new Claim(JwtClaimTypes.Scope, scope));
                claims.AddRange(scopes);
            }

            var expiration = issuedAt + context.Client.AccessTokenLifetime;
            claims.Add(new Claim(JwtClaimTypes.JwtId, jwtId));
            claims.Add(new Claim(JwtClaimTypes.NotBefore, issuedAt.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.IssuedAt, issuedAt.ToString(), ClaimValueTypes.Integer64));
            claims.Add(new Claim(JwtClaimTypes.Expiration, expiration.ToString(), ClaimValueTypes.Integer64));
            return claims;
        }

        private static IEnumerable<Claim> GetSubjectStandardClaims(string subject, long issuedAt, string authenticationMethod, string identityProvider)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, subject),
                new Claim(JwtClaimTypes.AuthenticationTime, issuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.IdentityProvider, identityProvider),
                new Claim(JwtClaimTypes.AuthenticationMethod, authenticationMethod)
            };
            return claims;
        }
    }
}
