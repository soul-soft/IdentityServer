namespace IdentityServer.Validation
{
    public interface IExtensionGrantsValidator
    {
        IEnumerable<string> GetExtensionGrantTypes();
        IExtensionGrantValidator? ValidateAsync(string grantType);
    }
}
