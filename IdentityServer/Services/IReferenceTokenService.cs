using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IReferenceTokenService
    {
        Task<IReferenceToken?> GetAsync(string id);
        Task<string> CreateAsync(IAccessToken token);
    }
}
