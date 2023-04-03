using Microsoft.AspNetCore.Authentication;

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
            if (_clock.UtcNow.UtcDateTime > authorizeCode.ExpirationTime)
            {
                await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizeCode);
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid lifetime");
            }
            await _authorizeCodeStore.RevomeAuthorizationCodeAsync(authorizeCode);
            return new GrantValidationResult(authorizeCode.Claims);
        }
    }
}
