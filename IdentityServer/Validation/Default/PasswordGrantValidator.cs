namespace IdentityServer.Validation
{
    internal class ResourceOwnerPasswordGrantValidator : IResourceOwnerPasswordGrantValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context)
        {
            return GrantValidationResult.ErrorAsync("Invalid username or password");
        }
    }
}
