using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IHandleGenerator _handleGenerator;

        public AuthenticationService(
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
            //request jwt
            var jwtId = await _handleGenerator.GenerateAsync(16);
            var issuer = _serverUrl.GetIdentityServerIssuerUri();
            var issuedAt = _systemClock.UtcNow.ToUnixTimeSeconds();
            var expiration = issuedAt + context.Client.AccessTokenLifetime;

            #region Standard Claims
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

            if (context.Claims.Any(a => a.Type == JwtClaimTypes.Subject))
            {
                claims.Add(context.Claims.Where(a => a.Type == JwtClaimTypes.Subject).First());
                claims.Add(new Claim(JwtClaimTypes.AuthenticationTime, issuedAt.ToString(), ClaimValueTypes.Integer64));
                claims.Add(new Claim(JwtClaimTypes.IdentityProvider, _options.IdentityProvider));
                claims.Add(new Claim(JwtClaimTypes.AuthenticationMethod, context.AuthenticationType));
            }
            #endregion

            #region Custom Claims
            var allowedRequestClaims = context.Claims
                .Where(a => context.Resources.ClaimTypes.Contains(a.Type))
                .Where(a => !Constants.ClaimTypeFilters.ClaimsServiceFilterClaimTypes.Contains(a.Type));
            claims.AddRange(allowedRequestClaims);
            #endregion

            return new ClaimsPrincipal(new ClaimsIdentity(claims, context.AuthenticationType));
        }
    }
}
