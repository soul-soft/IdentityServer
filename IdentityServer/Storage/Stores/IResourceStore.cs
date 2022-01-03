using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IResourceStore
    {
        Task<IEnumerable<Resource>> FindResourcesByScopeNameAsync(IEnumerable<string> scopeNames);
    }
}
