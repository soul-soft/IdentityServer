namespace IdentityServer.Validation
{
    public interface IResourceOwnerCredentialRequestValidator
    {
        Task ValidateAsync(ResourceOwnerCredentialValidation context);
    }
}
