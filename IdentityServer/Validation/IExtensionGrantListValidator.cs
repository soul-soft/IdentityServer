namespace IdentityServer.Validation
{
    public interface IExtensionGrantListValidator
    {
        IEnumerable<string> GetSupportedGrantTypes();
        Task<ExtensionGrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest request);
    }
}
