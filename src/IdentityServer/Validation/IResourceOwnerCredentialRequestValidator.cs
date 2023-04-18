namespace IdentityServer.Validation
{
    public interface IResourceOwnerCredentialRequestValidator
    {
        Task<GrantValidationResult> ValidateAsync(ResourceOwnerCredentialValidationRequest request);
    }
}
