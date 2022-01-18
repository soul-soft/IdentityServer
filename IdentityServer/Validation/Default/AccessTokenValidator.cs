using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Validation
{
    internal class AccessTokenValidator : IAccessTokenValidator
    {
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;
        private readonly IReferenceTokenService _referenceTokenService;

        public AccessTokenValidator(
            ISystemClock systemClock,
            IdentityServerOptions options,
            ISigningCredentialStore credentials,
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
                var issuer = _options.Issuer;
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
            var issuer = _options.Issuer;
            if (referenceToken.AccessToken.Issuer != issuer)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid issuer");
            }
            return referenceToken.AccessToken.ToClaims(_options);
        }
    }
}
