namespace IdentityServer.Services
{
    public interface ITokenEndpointAuthMethodProvider
    {
        IEnumerable<string> GetAllAuthMethods();
        Task<ITokenEndpointAuthMethod> GetDefaultAuthMethodAsync();
        Task<ITokenEndpointAuthMethod?> GetAuthMethodAsync(string name);
    }
}
