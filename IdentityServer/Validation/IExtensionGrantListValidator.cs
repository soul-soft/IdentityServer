namespace IdentityServer.Validation
{
    public interface IExtensionGrantListValidator
    {
        IEnumerable<string> GetGrantTypes();
        Task ValidateAsync(ExtensionGrantRequestValidation context);
    }
}
