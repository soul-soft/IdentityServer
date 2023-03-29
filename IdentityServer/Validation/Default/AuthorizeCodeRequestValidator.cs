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

        public async Task<AuthorizeCodeValidationResult> ValidateAsync(AuthorizeCodeValidationRequest request)
        {
            var authorizeCode = await _authorizeCodeStore.FindByAuthorizeCodeAsync(request.Code);
            if (authorizeCode == null)
            {
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Invalid refresh token");
            }
            if (_clock.UtcNow.UtcDateTime > authorizeCode.Expiration)
            {
                await _authorizeCodeStore.RevomeAuthorizeCodeAsync(authorizeCode.Id);
                throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Refresh token has expired");
            }
            await _authorizeCodeStore.RevomeAuthorizeCodeAsync(authorizeCode.Id);
            var claims = new ClaimsPrincipal(new ClaimsIdentity(authorizeCode.Claims, GrantTypes.AuthorizationCode));
            return new AuthorizeCodeValidationResult(claims);
        }
    }
}
