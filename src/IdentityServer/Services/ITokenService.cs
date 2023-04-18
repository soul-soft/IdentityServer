using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(Client client, ClaimsPrincipal subject);

        Task<string> CreateRefreshTokenAsync(Client client, ClaimsPrincipal subject);

        Task<string> CreateIdentityTokenAsync(Client client, ClaimsPrincipal subject, AuthorizationCode authorizationCode);
    }
}
