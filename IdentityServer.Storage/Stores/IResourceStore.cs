using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<string>> GetShowInDiscoveryDocumentScopesAsync();
        Task<Resources> FindByScopeAsync(IEnumerable<string> scopes);
    }
}
