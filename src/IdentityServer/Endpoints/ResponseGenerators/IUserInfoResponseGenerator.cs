using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public interface IUserInfoResponseGenerator
    {
        Task<UserInfoGeneratorResponse> GenerateAsync(ClaimsPrincipal subject, Client client, Resources resources);
    }
}
