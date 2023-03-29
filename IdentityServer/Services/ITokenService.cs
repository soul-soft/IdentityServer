using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(AccessTokenType accessTokenType, int lifetime, IEnumerable<string> algorithms, IEnumerable<Claim> claims);

        Task<string> CreateRefreshTokenAsync(IEnumerable<Claim> claims, int lifetime);
    }
}
