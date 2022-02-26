namespace IdentityServer.Validation
{
    internal class PasswordGrantValidator : IPasswordGrantValidator
    {
        public Task ValidateAsync(PasswordGrantValidationRequest context)
        {
            throw new InvalidGrantException("Invalid username or password");
        }
    }
}
