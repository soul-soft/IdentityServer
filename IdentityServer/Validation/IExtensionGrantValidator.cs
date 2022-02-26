namespace IdentityServer.Validation
{
    public interface IExtensionGrantValidator
    {
        string GrantType { get; }
        Task ValidateAsync(ExtensionGrantValidationRequest context);
    }
}
