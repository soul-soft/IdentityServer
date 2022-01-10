using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface ITokenService
    {
        Task<IToken> CreateAccessTokenAsync(TokenCreationRequest request);
        Task<IToken> CreateIdentityTokenAsync(TokenCreationRequest request);
        Task<string> CreateSecurityTokenAsync(IToken request);
    }
}
