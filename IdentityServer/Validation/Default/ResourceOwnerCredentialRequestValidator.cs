namespace IdentityServer.Validation
{
    internal class ResourceOwnerCredentialRequestValidator : IResourceOwnerCredentialRequestValidator
    {
        public Task ValidateAsync(ResourceOwnerCredentialValidation context)
        {
            throw new ValidationException(OpenIdConnectValidationErrors.InvalidGrant, "Invalid username or password");
        }
    }
}
