namespace IdentityServer.Validation
{
    public interface IExtensionGrantsListValidator
    {
        IEnumerable<string> GetExtensionGrantTypes();
        Task<GrantValidationResult> ValidateAsync(ExtensionGrantValidationContext context);
    }
}
