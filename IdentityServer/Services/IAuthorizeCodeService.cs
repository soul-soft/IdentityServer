using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IAuthorizeCodeService
    {        
        Task<string> GenerateCodeAsync(Client client, ClaimsPrincipal subject);
    }
}
