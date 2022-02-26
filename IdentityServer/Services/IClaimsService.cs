using System.Security.Claims;

namespace IdentityServer.Services
{
    public interface IClaimsService
    {
        Task<IEnumerable<Claim>> GetAccessTokenClaimsAsync(Client client, ResourceCollection resources);
        Task<IEnumerable<Claim>> GetIdentityTokenClaimsAsync(Client client, ResourceCollection resources);
    }
}
