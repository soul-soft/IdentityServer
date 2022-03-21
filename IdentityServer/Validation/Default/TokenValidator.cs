using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly ITokenStore _tokens;
        private readonly IClientStore _clients;
        private readonly IServerUrl _serverUrl;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IProfileService _profileService;
        private readonly ISigningCredentialStore _credentials;

        public TokenValidator(
            ITokenStore tokens,
            IClientStore clients,
            IServerUrl serverUrl,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IProfileService profileService,
            ISigningCredentialStore credentials)
        {
            _tokens = tokens;
            _clients = clients;
            _options = options;
            _serverUrl = serverUrl;
            _systemClock = systemClock;
            _credentials = credentials;
            _profileService = profileService;
        }

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidToken, "Access token too long");
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
                    return TokenValidationResult.Fail(OpenIdConnectValidationErrors.ExpiredToken, securityTokenExpiredException.Message);
                }
                else
                {
                    return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidToken, result.Exception.Message);
                }
            }
            return await ValidateSubjectAsync(result.ClaimsIdentity.Claims);
        }

        private async Task<TokenValidationResult> ValidateReferenceTokenAsync(string reference)
        {
            var token = await _tokens.FindTokenAsync(reference);
            if (token == null)
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidToken, "Invalid reference token");
            }
            if (token.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.ExpiredToken, "The access token has expired");
            }
            if (token.Issuer != _serverUrl.GetIdentityServerIssuerUri())
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidToken, "Invalid issuer");
            }
            return await ValidateSubjectAsync(token.Claims);
        }

        private async Task<TokenValidationResult> ValidateSubjectAsync(IEnumerable<Claim> claims)
        {
            if (_options.EmitScopesAsCommaDelimitedStringInJwt && claims.Any(a => a.Type == JwtClaimTypes.Scope))
            {
                var scopes = claims.Where(a => a.Type == JwtClaimTypes.Scope)
                    .First().Value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);

                claims = claims.Where(a => a.Type != JwtClaimTypes.Scope)
                    .Union(scopes.Select(scope => new Claim(JwtClaimTypes.Scope, scope)));
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "TokenValidator"));
            var clientId = subject.GetClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidToken, $"Token contains no client_id claim");
            }
            var client = await _clients.FindByClientIdAsync(clientId);
            if (client == null)
            {
                return TokenValidationResult.Fail(OpenIdConnectValidationErrors.InvalidClient, $"Client does not exist anymore.");
            }           
            return TokenValidationResult.Success(client, subject.Claims);
        }
    }
}
