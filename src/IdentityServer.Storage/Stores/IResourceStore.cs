using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync();
        Task<Resources> GetResourcesByScopesAsync(IEnumerable<string> scopes);
        Task<IEnumerable<ApiResource>> GetApiResourcesByNameAsync(string name);
    }
}

