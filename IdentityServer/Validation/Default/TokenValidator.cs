using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.Validation
{
    internal class TokenValidator : ITokenValidator
    {
        private readonly IServerUrl _urls;
        private readonly IClientStore _clients;
        private readonly ISystemClock _systemClock;
        private readonly IdentityServerOptions _options;
        private readonly IScopeValidator _scopeValidator;
        private readonly ISigningCredentialStore _credentials;
        private readonly IProfileService _profileService;
        private readonly IReferenceTokenService _referenceTokenService;

        public TokenValidator(
            IServerUrl urls,
            IClientStore clients,
            ISystemClock systemClock,
            IdentityServerOptions options,
            IScopeValidator scopeValidator,
            IProfileService profileService,
            ISigningCredentialStore credentials,
            IReferenceTokenService referenceTokenService)
        {
            _urls = urls;
            _options = options;
            _clients = clients;
            _scopeValidator = scopeValidator;
            _systemClock = systemClock;
            _credentials = credentials;
            _profileService = profileService;
            _referenceTokenService = referenceTokenService;
        }

        public async Task<TokenValidationResult> ValidateAccessTokenAsync(string? token)
        {
            TokenValidationResult validationResult;
            if (string.IsNullOrWhiteSpace(token))
            {
                return TokenValidationResult.Error("Access token is missing");
            }
            if (token.Length > _options.InputLengthRestrictions.AccessToken)
            {
                return TokenValidationResult.Error("Access token too long");
            }
            if (token.Contains('.'))
            {
                validationResult = await ValidateJwtTokenAsync(token);
            }
            else
            {
                validationResult = await ValidateReferenceTokenAsync(token);
            }
            var isActive = new IsActiveContext(
                validationResult.Client,
                validationResult.Subject,
                ProfileIsActiveCaller.AccessTokenValidation);
            await _profileService.IsActiveAsync(isActive);
            if (!isActive.IsActive)
            {
                var sub = validationResult.Subject.GetSubjectId();
                return TokenValidationResult.Error("User marked as not active: {subject}", sub);
            }
            return validationResult;
        }

        private async Task<TokenValidationResult> ValidateJwtTokenAsync(string token)
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
                var subject = handler.ValidateToken(token, parameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null)
                {
                    return TokenValidationResult.Error("The access token is not a valid JWT type");
                }
                var clidentId = subject.FindFirstValue(JwtClaimTypes.ClientId);
                if (string.IsNullOrWhiteSpace(clidentId))
                {
                    return TokenValidationResult.Error("Client Id is missing");
                }
                var client = await _clients.GetAsync(clidentId);
                if (client == null)
                {
                    return TokenValidationResult.Error("Client deleted or disabled: {clientId}", clidentId);
                }
                var result = await ValidateClientAsync(client, subject);
                if (result.IsError)
                {
                    return TokenValidationResult.Error(result.Description);
                }
                return TokenValidationResult.Success(client, subject);
            }
            catch (Exception ex)
            {
                return TokenValidationResult.Error(ex.Message);
            }
        }

        private async Task<TokenValidationResult> ValidateReferenceTokenAsync(string token)
        {
            var referenceToken = await _referenceTokenService.GetAsync(token);
            if (referenceToken == null || referenceToken.Expiration < _systemClock.UtcNow.UtcDateTime)
            {
                return TokenValidationResult.Error("The access token has expired");
            }
            var issuer = _urls.GetIdentityServerIssuerUri();
            if (referenceToken.Token.Issuer != issuer)
            {
                return TokenValidationResult.Error("Invalid issuer");
            }
            var claims = referenceToken.Token.ToClaims(_options);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var clidentId = subject.FindFirstValue(JwtClaimTypes.ClientId);
            if (string.IsNullOrWhiteSpace(clidentId))
            {
                return TokenValidationResult.Error("Client Id is missing");
            }
            var client = await _clients.GetAsync(clidentId);
            if (client == null)
            {
                return TokenValidationResult.Error("Client deleted or disabled: {clientId}", clidentId);
            }
            var result = await ValidateClientAsync(client, subject);
            if (result.IsError)
            {
                return TokenValidationResult.Error(result.Description);
            }
            return TokenValidationResult.Success(client, subject);
        }

        private async Task<ValidationResult> ValidateClientAsync(IClient client, ClaimsPrincipal subject)
        {
            var sub = subject.GetSubjectId();
            if (string.IsNullOrWhiteSpace(sub))
            {
                return ValidationResult.Error("Sub is missing");
            }
            var scopes = subject.FindAll(JwtClaimTypes.Scope)
                   .Select(s => s.Value)
                   .Where(s => !string.IsNullOrWhiteSpace(s));
            var validationResult = await _scopeValidator.ValidateAsync(client.AllowedScopes, scopes);
            if (validationResult.IsError)
            {
                return ValidationResult.Error(validationResult.Description);
            }
            return ValidationResult.Success();
        }
    }
}
