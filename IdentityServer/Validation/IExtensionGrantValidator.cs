namespace IdentityServer.Validation
{
    public interface IExtensionGrantValidator
    {
        string GrantType { get; }
        Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest request);
    }
}
