using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync();
        Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(string name);
        Task<ResourceCollection> FindResourcesByScopesAsync(IEnumerable<string> scopes);
    }
}
