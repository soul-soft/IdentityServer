using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface IReferenceTokenService
    {
        Task<string> CreateAsync(IToken token);
    }
}
