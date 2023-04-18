namespace IdentityServer.Validation
{
    internal class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request)
        {
            throw new ValidationException(ValidationErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
