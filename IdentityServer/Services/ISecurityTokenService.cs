using System.IdentityModel.Tokens.Jwt;
using IdentityServer.Models;

namespace IdentityServer.Services
{
    public interface ISecurityTokenService
    {
        Task<string> CreateTokenAsync(TokenRequest request);
    }
}
