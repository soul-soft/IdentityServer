using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class AuthorizeCodeRequestValidator : IAuthorizeCodeRequestValidator
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizationCodeStore _authorizeCodeStore;

        public AuthorizeCodeRequestValidator(
            ISystemClock clock,
            IAuthorizationCodeStore authorizeCodeStore)
        {
            _clock = clock;
            _authorizeCodeStore = authorizeCodeStore;
        }

        public async Task<GrantValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request)
        {
            var authorizeCode = await _authorizeCodeStore.FindAuthorizationCodeAsync(request.Code);
            if (authorizeCode == null)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid code");
            }
            if (authorizeCode.ExpirationTime < _clock.UtcNow.UtcDateTime)
            {
                await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizeCode);
                throw new ValidationException(ValidationErrors.InvalidGrant, "Code expired");
            }
            await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizeCode);
            var subject = new ClaimsPrincipal(new ClaimsIdentity(authorizeCode.Claims, GrantTypes.AuthorizationCode));
            return new GrantValidationResult(subject, authorizeCode);
        }
    }
}
