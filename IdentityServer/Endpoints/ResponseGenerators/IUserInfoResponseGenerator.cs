using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public interface IUserInfoResponseGenerator
    {
        Task<UserInfoResponse> ProcessAsync(ClaimsPrincipal subject, Client client, Resources resources);
    }
}
