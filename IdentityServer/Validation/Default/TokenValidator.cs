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
        private readonly IdentityServerOptions _options;
        private readonly ISigningCredentialsService _credentials;

        public TokenValidator(
            ITokenStore tokens,
            IClientStore clients,
            IServerUrl serverUrl,
            IdentityServerOptions options,
            ISigningCredentialsService credentials)
        {
            _tokens = tokens;
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
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidIssuer = _serverUrl.GetIdentityServerIssuerUri(),
                IssuerSigningKeys = securityKeys,
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

        private async Task<TokenValidationResult> ValidateReferenceTokenAsync(string reference)
        {
            var token = await _tokens.FindTokenAsync(reference);
            if (token == null)
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Invalid reference token");
            }
            if (token.GetIssuer() != _serverUrl.GetIdentityServerIssuerUri())
            {
                return TokenValidationResult.Fail(ValidationErrors.InvalidToken, "Invalid issuer");
            }
            var result = await ValidateClaimsAsync(token.Claims);
            await _tokens.SetLifetimeAsync(token);
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
