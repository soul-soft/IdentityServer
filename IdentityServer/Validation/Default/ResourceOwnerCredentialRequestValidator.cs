namespace IdentityServer.Validation
{
    internal class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task<ResourceOwnerCredentialValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request)
        {
            throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
