using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialsStore _credentials;
        private readonly IReferenceTokenService _referenceTokenService;

        public TokenValidator(
            ISystemClock systemClock,
            IdentityServerOptions options,
            ISigningCredentialsStore credentials,
            IReferenceTokenService referenceTokenService)
        {
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
            return claims;
        }

        private async Task<IEnumerable<Claim>> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                var securityKeys = await _credentials.GetSecurityKeysAsync();
                var parameters = new TokenValidationParameters
                {
                    ValidateAudience = _options.TokenValidations.ValidateAudience,
                    ValidAudience = _options.TokenValidations.Audience,
                    ValidateLifetime = _options.TokenValidations.ValidateLifetime,
                    ValidIssuer = _options.Issuer,
                    ValidateIssuer = _options.TokenValidations.ValidateIssuer,
                    IssuerSigningKeys = securityKeys,
                };
                var subject = handler.ValidateToken(token, parameters, out var securityToken);
                if (_options.TokenValidations.ValidateScope)
                {
                    var scopes = subject.Claims
                        .Where(a => a.Type == JwtClaimTypes.Scope).Select(s => s.Value);
                    if (!scopes.Contains(_options.TokenValidations.Scope))
                    {
                        throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid scope");
                    }
                }
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
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid token");
            }
            if (_options.TokenValidations.ValidateLifetime && referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "The access token has expired");
            }
            if (_options.TokenValidations.ValidateAudience && !referenceToken.AccessToken.Audiences.Contains(_options.TokenValidations.Audience))
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid audience");
            }
            if (_options.TokenValidations.ValidateIssuer && referenceToken.AccessToken.Issuer != _options.Issuer)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid issuer");
            }
            if (_options.TokenValidations.ValidateScope && !referenceToken.AccessToken.Scopes.Contains(_options.TokenValidations.Scope))
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid scope");
            }
            return referenceToken.AccessToken.ToClaims(_options);
        }
    }
}
