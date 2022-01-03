namespace IdentityServer.Application
{
    public interface IExtensionGrantValidator
    {
        Task<ValidationResult> ValidateAsync(ExtensionGrantRequest context);
    }
}
