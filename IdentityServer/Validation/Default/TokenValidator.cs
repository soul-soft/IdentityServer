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

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                return TokenValidationResult.Fail(OpenIdConnectErrors.InvalidToken, "Access token too long");
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

        private async Task<TokenValidationResult> ValidateJwtTokenAsync(string token)
        {
            var handler = new JsonWebTokenHandler();
            var securityKeys = await _credentials.GetSecurityKeysAsync();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidIssuer = _serverUrl.GetIdentityServerIssuerUri(),
                IssuerSigningKeys = securityKeys,
            };
            var result = handler.ValidateToken(token, parameters);
            if (!result.IsValid)
            {
                if (result.Exception is SecurityTokenExpiredException securityTokenExpiredException)
                {
                    return TokenValidationResult.Fail(OpenIdConnectErrors.ExpiredToken, securityTokenExpiredException.Message);
                }
                else
                {
                    return TokenValidationResult.Fail(OpenIdConnectErrors.InvalidToken, result.Exception.Message);
                }
            }
            return TokenValidationResult.Success(result.ClaimsIdentity.Claims);
        }

        private async Task<TokenValidationResult> ValidateReferenceTokenAsync(string tokenReference)
        {
            var token = await _referenceTokenStore.FindTokenAsync(tokenReference);
            if (token == null)
            {
                return TokenValidationResult.Fail(OpenIdConnectErrors.InvalidToken, "Invalid reference token");
            }
            if (token.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                return TokenValidationResult.Fail(OpenIdConnectErrors.ExpiredToken, "The access token has expired");
            }
            if (token.Issuer != _serverUrl.GetIdentityServerIssuerUri())
            {
                return TokenValidationResult.Fail(OpenIdConnectErrors.InvalidToken, "Invalid issuer");
            }
            var claims = ValidateClaims(token.GetJwtClaims());
            return TokenValidationResult.Success(claims);
        }

        public IEnumerable<Claim> ValidateClaims(IEnumerable<Claim> claims)
        {
            if (_options.EmitScopesAsCommaDelimitedStringInJwt && claims.Any(a => a.Type == JwtClaimTypes.Scope))
            {
                var scopes = claims.Where(a => a.Type == JwtClaimTypes.Scope)
                    .First().Value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);

                return claims.Where(a => a.Type != JwtClaimTypes.Scope)
                    .Union(scopes.Select(scope => new Claim(JwtClaimTypes.Scope, scope)));
            }
            return claims;
        }
    }
}
