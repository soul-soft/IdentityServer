using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<Resources> FindResourcesByScopeAsync(IEnumerable<string> scopes);
    }
}
