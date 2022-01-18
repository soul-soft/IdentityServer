using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Validation
{
    internal class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly IServerUrl _urls;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;
        private readonly IReferenceTokenService _referenceTokenService;

        public AccessTokenValidator(
            IServerUrl urls,
            ISystemClock systemClock,
            IdentityServerOptions options,
            ISigningCredentialStore credentials,
            IReferenceTokenService referenceTokenService)
        {
            _urls = urls;
            _options = options;
            _systemClock = systemClock;
            _credentials = credentials;
            _referenceTokenService = referenceTokenService;
        }

        public async Task<IEnumerable<Claim>> ValidateAsync(string? token)
        {
            IEnumerable<Claim> claims;
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Access token is missing");
            }
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Access token too long");
            }
            if (token.Contains('.'))
            {
                claims = await ValidateJwtTokenAsync(token);
            }
            else
            {
                claims = await ValidateReferenceTokenAsync(token);
            }
            if (_options.EmitScopesAsSpaceDelimitedStringInJwt)
            {
                var scope = claims
                    .Where(a => a.Type == JwtClaimTypes.Scope)
                    .FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(scope))
                {
                    var scopes = scope.Split(',');
                    var list = claims.Where(a => a.Type != JwtClaimTypes.Scope).ToList();
                    foreach (var item in scopes)
                    {
                        list.Add(new Claim(JwtClaimTypes.Scope, item));
                    }
                    claims = list;
                }
            }
            return claims;
        }

        private async Task<IEnumerable<Claim>> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                var securityKeys = await _credentials.GetSecurityKeysAsync();
                var issuer = _urls.GetIssuerUri();
                var parameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = _options.Issuer,
                    IssuerSigningKeys = securityKeys,
                };
                var subject = handler.ValidateToken(token, parameters, out var securityToken);
                return subject.Claims;
            }
            catch (Exception ex)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, ex.Message);
            }
        }

        private async Task<IEnumerable<Claim>> ValidateReferenceTokenAsync(string token)
        {
            var referenceToken = await _referenceTokenService.GetAsync(token);
            if (referenceToken == null || referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "The access token has expired");
            }
            var issuer = _urls.GetIssuerUri();
            if (referenceToken.AccessToken.Issuer != issuer)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid issuer");
            }
            return referenceToken.AccessToken.ToClaims(_options);
        }
    }
}
