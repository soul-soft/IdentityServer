namespace IdentityServer.Validation
{
    public interface IResourceOwnerCredentialRequestValidator
    {
        Task<ResourceOwnerCredentialValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request);
    }
}
