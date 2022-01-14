using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IReferenceTokenStore
    {
        Task SaveAsync(IReferenceToken token);
        Task<IReferenceToken?> FindReferenceTokenByIdAsync(string id);
    }
}
