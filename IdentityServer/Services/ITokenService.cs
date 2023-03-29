using IdentityServer.Storage.Models;
using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateAccessTokenAsync(Token token);
        Task<Token> CreateTokenAsync(Client client, ClaimsPrincipal subject);
        Task<string> CreateRefreshTokenAsync(Token token, int refreshTokenLifetime);
    }
}
