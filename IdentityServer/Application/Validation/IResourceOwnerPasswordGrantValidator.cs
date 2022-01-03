namespace IdentityServer.Application
{
    public interface IResourceOwnerPasswordGrantValidator
    {
        Task<ValidationResult> ValidateAsync(ResourceOwnerPasswordGrantRequest context);
    }
}
