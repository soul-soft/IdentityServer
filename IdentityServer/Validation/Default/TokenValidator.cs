using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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

        public async Task<ClaimsPrincipal> ValidateAsync(string? token)
        {
            IEnumerable<Claim> claims;
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidTokenException("Access token is missing");
            }
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                throw new InvalidTokenException("Access token too long");
            }
            if (token.Contains('.'))
            {
                claims = await ValidateJwtTokenAsync(token);
            }
            else
            {
                claims = await ValidateReferenceTokenAsync(token);
            }
            var identity = new ClaimsIdentity(_options.Authentications.Scheme);
            identity.AddClaims(claims);
            return new ClaimsPrincipal(identity);
        }

        private async Task<IEnumerable<Claim>> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                handler.InboundClaimTypeMap.Clear();
                var securityKeys = await _credentials.GetSecurityKeysAsync();
                var tokenValidationParameters = _options.Authentications.TokenValidationParameters;
                var parameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidIssuer = _options.Issuer,
                    ValidateIssuer = _options.Authentications.TokenValidationParameters.ValidateIssuer,
                    ValidateAudience = _options.Authentications.TokenValidationParameters.ValidateAudience,
                    ValidAudience = _options.Authentications.TokenValidationParameters.ValidAudience,
                    ValidateLifetime = _options.Authentications.TokenValidationParameters.ValidateLifetime,
                    IssuerSigningKeys = securityKeys,
                };
                var subject = handler.ValidateToken(token, parameters, out var securityToken);
                return subject.Claims;
            }
            catch (Exception ex)
            {
                throw new InvalidTokenException(ex.Message);
            }
        }

        private async Task<IEnumerable<Claim>> ValidateReferenceTokenAsync(string token)
        {
            var referenceToken = await _referenceTokenService.GetAsync(token);
            var tokenValidationParameters = _options.Authentications.TokenValidationParameters;
            if (referenceToken == null || referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidTokenException("Invalid token");
            }
            if (tokenValidationParameters.ValidateLifetime && referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new InvalidTokenException("The access token has expired");
            }
            if (tokenValidationParameters.ValidateAudience && !referenceToken.AccessToken.Audiences.Contains(tokenValidationParameters.ValidAudience))
            {
                throw new InvalidTokenException("Invalid audience");
            }
            if (tokenValidationParameters.ValidateIssuer && referenceToken.AccessToken.Issuer != _options.Issuer)
            {
                throw new InvalidTokenException("Invalid issuer");
            }
            
            return referenceToken.AccessToken.ToClaims(_options);
        }
    }
}
