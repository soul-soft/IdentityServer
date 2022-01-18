namespace IdentityServer.Validation
{
    internal class PasswordGrantValidator : IPasswordGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context)
        {
            throw new InvalidGrantException("Invalid username or password");
        }
    }
}
