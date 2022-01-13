namespace IdentityServer.Validation
{
    public interface IResourceOwnerPasswordGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantValidationContext context);
    }
}
