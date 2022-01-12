namespace IdentityServer.Validation
{
    public interface IExtensionGrantsValidator
    {
        IEnumerable<string> GetExtensionGrantTypes();
        IExtensionGrantValidator? GetExtensionGrantValidator(string grantType);
    }
}
