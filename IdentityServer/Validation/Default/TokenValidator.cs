using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialStore _credentials;
        private readonly ITokenStore _referenceTokenStore;

        public TokenValidator(
            IServerUrl serverUrl,
            ISystemClock systemClock,
            IdentityServerOptions options,
            ISigningCredentialStore credentials,
            ITokenStore referenceTokenStore)
        {
            _options = options;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _credentials = credentials;
            _referenceTokenStore = referenceTokenStore;
        }

        public async Task<IEnumerable<Claim>> ValidateAccessTokenAsync(string token)
        {
            IEnumerable<Claim> claims;
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidToken, "Access token too long");
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
            var handler = new JsonWebTokenHandler();
            var securityKeys = await _credentials.GetSecurityKeysAsync();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = _serverUrl.GetIssuerUrl(),
                IssuerSigningKeys = securityKeys,
            };
            var result = handler.ValidateToken(token, parameters);
            if (!result.IsValid)
            {
                if (result.Exception is SecurityTokenExpiredException securityTokenExpiredException)
                {
                    throw new ValidationException(OpenIdConnectErrors.ExpiredToken, securityTokenExpiredException.Message);
                }
                else
                {
                    throw new ValidationException(OpenIdConnectErrors.InvalidToken, result.Exception.Message);
                }
            }
            return result.ClaimsIdentity.Claims;
        }

        private async Task<IEnumerable<Claim>> ValidateReferenceTokenAsync(string tokenReference)
        {
            var token = await _referenceTokenStore.FindTokenAsync(tokenReference);
            if (token == null)
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidToken, "Invalid reference token");
            }
            if (token.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                throw new ValidationException(OpenIdConnectErrors.ExpiredToken, "The access token has expired");
            }
            if (token.Issuer != _serverUrl.GetIssuerUrl())
            {
                throw new ValidationException(OpenIdConnectErrors.InvalidToken, "Invalid issuer");
            }
            return token.GetJwtClaims();
        }
    }
}
