using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<string>> GetScopesAsync();
        Task<Resources> FindResourcesByScopeAsync(IEnumerable<string> scopes);
    }
}
