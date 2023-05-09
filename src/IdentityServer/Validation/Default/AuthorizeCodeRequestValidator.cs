using IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            var codeChallenge = CodeChallengeHash(authorizationCode.CodeChallengeMethod!, request.CodeVerifier!);
            if (authorizationCode.CodeChallenge != codeChallenge)
            {
                throw new ValidationException(ValidationErrors.InvalidCodeVerifier, "code_verifier validation failed");
            }
        }

        private string CodeChallengeHash(string method, string codeVerifier)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(codeVerifier);
                var hash = sha.ComputeHash(bytes);
                return Base64UrlEncoder.Encode(hash);
            }
        }
    }
}
