using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IReferenceTokenStore
    {
        Task StoreTokenAsync(ReferenceToken token);
        Task<ReferenceToken?> FindTokenAsync(string id);
    }
}
