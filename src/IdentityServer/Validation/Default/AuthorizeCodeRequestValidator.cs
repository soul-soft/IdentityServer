using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer.Validation
{
    internal class AuthorizeCodeRequestValidator : IAuthorizeCodeRequestValidator
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizationCodeStore _authorizeCodeStore;
        private readonly ICodeChallengeHashService _codeChallengeHashService;

        public AuthorizeCodeRequestValidator(
            ISystemClock clock,
            IAuthorizationCodeStore authorizeCodeStore,
            ICodeChallengeHashService codeChallengeHashService)
            
        {
            _clock = clock;
            _authorizeCodeStore = authorizeCodeStore;
            _codeChallengeHashService = codeChallengeHashService;
        }

        public async Task<GrantValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request)
        {
            var authorizationCode = await _authorizeCodeStore.FindAuthorizationCodeAsync(request.Code);
            if (authorizationCode == null)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid code");
            }
            if (authorizationCode.ExpirationTime < _clock.UtcNow.UtcDateTime)
            {
                await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizationCode);
                throw new ValidationException(ValidationErrors.InvalidGrant, "Code expired");
            }
            if (!string.IsNullOrEmpty(authorizationCode.CodeChallenge))
            {
                ValidateCodeChallenge(request, authorizationCode);
            }
            await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizationCode);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(authorizationCode.Claims, GrantTypes.AuthorizationCode));
            return new GrantValidationResult(subject, authorizationCode);
        }

        private void ValidateCodeChallenge(AuthorizeCodeValidationRequest request, AuthorizationCode authorizationCode)
        {
            if (string.IsNullOrEmpty(request.CodeVerifier))
            {
                throw new ValidationException(ValidationErrors.InvalidRequest, "code_verifier is required");
            }
            var codeChallenge = _codeChallengeHashService.ComputeHash(request.CodeVerifier!, authorizationCode.CodeChallengeMethod!);
            if (authorizationCode.CodeChallenge != codeChallenge)
            {
                throw new ValidationException(ValidationErrors.InvalidCodeVerifier, "code_verifier validation failed");
            }
        }
    }
}
