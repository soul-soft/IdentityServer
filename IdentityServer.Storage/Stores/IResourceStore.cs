using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<Resources> FindResourcesAsync(IEnumerable<string> scopes);
        Task<IEnumerable<ApiResource>> FindApiResourcesAsync(string name);
        Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync();
    }
}

