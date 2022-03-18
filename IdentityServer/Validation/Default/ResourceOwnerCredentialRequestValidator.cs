namespace IdentityServer.Validation
{
    internal class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task<ResourceOwnerCredentialValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request)
        {
            throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
