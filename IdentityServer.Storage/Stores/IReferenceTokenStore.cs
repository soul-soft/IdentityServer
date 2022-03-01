using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IReferenceTokenStore
    {
        Task StoreReferenceTokenAsync(Token token);
        Task<ReferenceToken?> FindReferenceTokenAsync(string id);
    }
}
