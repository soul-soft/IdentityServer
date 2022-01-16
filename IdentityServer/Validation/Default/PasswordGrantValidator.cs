namespace IdentityServer.Validation
{
    internal class ResourceOwnerPasswordGrantValidator : IResourceOwnerPasswordGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context)
        {
            throw new InvalidGrantException("Invalid username or password");
        }
    }
}
