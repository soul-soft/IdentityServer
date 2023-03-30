namespace IdentityServer.Validation
{
    public interface IExtensionGrantListValidator
    {
        IEnumerable<string> GetSupportedGrantTypes();
        Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationRequest request);
    }
}
