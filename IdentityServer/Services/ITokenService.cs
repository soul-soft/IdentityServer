using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(Token token);
        Task<string> CreateRefreshTokenAsync(Token token, int refreshTokenLifetime);
        Task<Token> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject);
    }
}
