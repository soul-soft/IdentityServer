namespace IdentityServer.Validation
{
    internal class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task<GrantValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request)
        {
            throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
