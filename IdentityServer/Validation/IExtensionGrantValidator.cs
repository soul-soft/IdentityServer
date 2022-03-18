namespace IdentityServer.Validation
{
    public interface IExtensionGrantValidator
    {
        string GrantType { get; }
        Task<ExtensionGrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest  context);
    }
}
