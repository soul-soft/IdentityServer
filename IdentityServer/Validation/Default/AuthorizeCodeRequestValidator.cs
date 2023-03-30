using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    internal class AuthorizeCodeRequestValidator : IAuthorizeCodeRequestValidator
    {
        private readonly ISystemClock _clock;
        private readonly IAuthorizeCodeStore _authorizeCodeStore;

        public AuthorizeCodeRequestValidator(
            ISystemClock clock,
            IAuthorizeCodeStore authorizeCodeStore)
        {
            _clock = clock;
            _authorizeCodeStore = authorizeCodeStore;
        }

        public async Task<GrantValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request)
        {
            var authorizeCode = await _authorizeCodeStore.FindByAuthorizeCodeAsync(request.Code);
            if (authorizeCode == null)
            {
                throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            if (_clock.UtcNow.UtcDateTime > authorizeCode.Expiration)
            {
                await _authorizeCodeStore.RevomeAuthorizeCodeAsync(authorizeCode.Id);
                throw new ValidationException(ValidationErrors.InvalidGrant, "Refresh token has expired");
            }
            await _authorizeCodeStore.RevomeAuthorizeCodeAsync(authorizeCode.Id);
            return new GrantValidationResult(authorizeCode.Claims);
        }
    }
}
