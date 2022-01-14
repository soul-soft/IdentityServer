using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IReferenceTokenService
    {
        Task<IReferenceToken?> GetAsync(string id);
        Task<string> CreateAsync(IToken token);
        ClaimsPrincipal CreateClaimsPrincipal(IReferenceToken token);
    }
}
