using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IReferenceTokenStore
    {
        Task<string> StoreReferenceTokenAsync(Token token);
        Task<ReferenceToken?> FindReferenceTokenAsync(string id);
    }
}
