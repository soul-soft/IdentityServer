using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IAuthorizeCodeService
    {        
        Task<string> CreateAuthorizeCodeAsync(Client client, ClaimsPrincipal subject);
    }
}
