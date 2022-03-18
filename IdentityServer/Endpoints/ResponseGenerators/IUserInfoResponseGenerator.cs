using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public interface IUserInfoResponseGenerator
    {
        Task<UserInfoGeneratorResponse> ProcessAsync(ClaimsPrincipal subject, Client client, Resources resources);
    }
}
