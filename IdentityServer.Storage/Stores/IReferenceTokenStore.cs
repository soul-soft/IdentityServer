using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IReferenceTokenStore
    {
        Task AddAsync(ReferenceToken token);
        Task<ReferenceToken?> FindByIdAsync(string id);
    }
}
