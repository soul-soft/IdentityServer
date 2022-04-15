using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class SignInService : ISignInService
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IHandleGenerator _handleGenerator;

        public SignInService(
            IServerUrl serverUrl,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IHandleGenerator handleGenerator)
        {
            _options = options;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _handleGenerator = handleGenerator;
        }

        public async Task<ClaimsPrincipal> SingInAsync(SingInAuthenticationContext context)
        {
            #region Jwt Claims
            //request jwt
            var jwtId = await _handleGenerator.GenerateAsync(16);
            var issuer = _serverUrl.GetIdentityServerIssuerUri();
            var issuedAt = _systemClock.UtcNow.ToUnixTimeSeconds();
            var expiration = issuedAt + context.Client.AccessTokenLifetime;
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.JwtId, jwtId),
                new Claim(JwtClaimTypes.Issuer, issuer),
                new Claim(JwtClaimTypes.ClientId, context.Client.ClientId),
                new Claim(JwtClaimTypes.IssuedAt, issuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.NotBefore, issuedAt.ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtClaimTypes.Expiration, expiration.ToString(), ClaimValueTypes.Integer64)
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

            if (context.Subject.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                claims.AddRange(GetStandardSubjectClaims(context.Subject));
            }
            #endregion

            #region Subject Claims
            var allowedRequestedClaims = context.Subject.Claims
                .Where(a => context.Resources.ClaimTypes.Contains(a.Type))
                .Where(a => !Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
            claims.AddRange(allowedRequestedClaims);
            #endregion

            return new ClaimsPrincipal(new ClaimsIdentity(claims, context.AuthenticationType));
        }
       
        private static IEnumerable<Claim> GetStandardSubjectClaims(ClaimsPrincipal subject)
        {
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.Subject).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.IdentityProvider).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationTime).First();
            yield return subject.Claims.Where(a => a.Type == JwtClaimTypes.AuthenticationMethod).First();
        }
    }
}
