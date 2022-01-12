using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<IToken> CreateAccessTokenAsync(TokenRequest request);
        Task<IToken> CreateIdentityTokenAsync(TokenRequest request);
        Task<string> CreateSecurityTokenAsync(IToken request);
    }
}
