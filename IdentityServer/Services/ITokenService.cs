using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(ReferenceToken token);
        Task<string> CreateRefreshTokenAsync(ReferenceToken token, int refreshTokenLifetime);
        Task<ReferenceToken> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject);
    }
}
