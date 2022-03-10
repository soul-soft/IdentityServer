namespace IdentityServer.Validation
{
    internal class PasswordGrantValidator : IPasswordGrantValidator
    {
        public Task ValidateAsync(PasswordGrantValidationRequest context)
        {
            throw new ValidationException(OpenIdConnectErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
