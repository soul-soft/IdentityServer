using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly ITokenStore _tokens;
        private readonly ISystemClock _clock;
        private readonly IClientStore _clients;
        private readonly IServerUrl _serverUrl;
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialsService _credentials;

        public TokenValidator(
            ITokenStore tokens,
            ISystemClock clock,
            IClientStore clients,
            IServerUrl serverUrl,
            IdentityServerOptions options,
            ISigningCredentialsService credentials)
        {
            _tokens = tokens;
            _clock = clock;
            _clients = clients;
            _options = options;
            _serverUrl = serverUrl;
            _credentials = credentials;
        }

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string token)
        {
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Access token too long");
            }
            if (token.Contains('.'))
            {
                return await ValidateJwtTokenAsync(token);
            }
            else
            {
                var refToken = await _tokens.FindAccessTokenAsync(token);
                var result = await ValidateReferenceTokenAsync(refToken);
                if (refToken != null)
                {
                    if (result.IsError)
                    {
                        await _tokens.RevomeTokenAsync(refToken);
                    }
                    else 
                    {
                        refToken.ExpirationTime = _clock.UtcNow.UtcDateTime.AddSeconds(refToken.Lifetime);
                        await _tokens.SaveTokenAsync(refToken);
                    }
                }
                return result;
            }
        }

        private async Task<TokenValidationResult> ValidateJwtTokenAsync(string token)
        {
            var handler = new JsonWebTokenHandler();
            var issuer = _serverUrl.GetIdentityServerIssuer();
            var signingKeys = await _credentials.GetSecurityKeysAsync();
            var parameters = new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                IssuerSigningKeys = signingKeys,
            };
            var result = handler.ValidateToken(token, parameters);
            if (!result.IsValid)
            {
                if (result.Exception is SecurityTokenExpiredException)
                {
                    return TokenValidationResult.Fail(ValidationErrors.ExpiredToken, "Token expired");
                }
                else
                {
                    return TokenValidationResult.Fail(ValidationErrors.InvalidToken, result.Exception.Message);
                }
            }
            return await ValidateClaimsAsync(result.ClaimsIdentity.Claims);
        }

        private async Task<TokenValidationResult> ValidateReferenceTokenAsync(Token? token)
        {
            var issuer = _serverUrl.GetIdentityServerIssuer();
            if (token == null)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Invalid reference token");
            }
            if (token.ExpirationTime < _clock.UtcNow.UtcDateTime)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Token expired");
            }
            if (token.GetIssuer() != issuer)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Invalid issuer");
            }
            var result = await ValidateClaimsAsync(token.Claims);
            return result;
        }

        private async Task<TokenValidationResult> ValidateClaimsAsync(IEnumerable<Claim> claims)
        {
            if (_options.EmitScopesAsCommaDelimitedStringInJwt && claims.Any(a => a.Type == JwtClaimTypes.Scope))
            {
                var scopes = claims.Where(a => a.Type == JwtClaimTypes.Scope)
                    .First().Value
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);

                claims = claims.Where(a => a.Type != JwtClaimTypes.Scope)
                    .Union(scopes.Select(scope => new Claim(JwtClaimTypes.Scope, scope)));
            }
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims, "Local"));
            var clientId = subject.GetClientId();
            if (string.IsNullOrEmpty(clientId))
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, $"Token contains no client_id claim");
            }
            var client = await _clients.FindClientAsync(clientId);
            if (client == null)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidClient, $"Client does not exist anymore");
            }
            return TokenValidationResult.Success(client, subject.Claims);
        }
    }
}
