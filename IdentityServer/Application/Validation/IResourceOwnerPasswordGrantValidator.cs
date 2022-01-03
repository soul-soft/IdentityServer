namespace IdentityServer.Application
{
    public interface IResourceOwnerPasswordGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(ResourceOwnerPasswordGrantRequest context);
    }
}
