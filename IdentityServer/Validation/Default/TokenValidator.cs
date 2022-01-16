using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly IServerUrl _urls;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;
        private readonly IReferenceTokenService _referenceTokenService;

        public TokenValidator(
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

        public async Task<ClaimsPrincipal> ValidateAccessTokenAsync(string? token)
        {
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
                return await ValidateJwtTokenAsync(token);
            }
            else
            {
                return await ValidateReferenceTokenAsync(token);
            }           
        }

        private async Task<ClaimsPrincipal> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                var securityKeys = await _credentials.GetSecurityKeysAsync();
                var issuer = _urls.GetIdentityServerIssuerUri();
                var parameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidIssuer = issuer,
                    IssuerSigningKeys = securityKeys,
                };
                return handler.ValidateToken(token, parameters, out var securityToken);
            }
            catch (Exception ex)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, ex.Message);
            }
        }

        private async Task<ClaimsPrincipal> ValidateReferenceTokenAsync(string token)
        {
            var referenceToken = await _referenceTokenService.GetAsync(token);
            if (referenceToken == null || referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "The access token has expired");
            }
            var issuer = _urls.GetIdentityServerIssuerUri();
            if (referenceToken.AccessToken.Issuer != issuer)
            {
                throw new InvalidException(OpenIdConnectTokenErrors.InvalidToken, "Invalid issuer");
            }
            var claims = referenceToken.AccessToken.ToClaims(_options);
            var identity = new ClaimsIdentity(claims,"Reference");
            return new ClaimsPrincipal(identity);
        }
    }
}
