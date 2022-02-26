using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IClientStore
    {
        Task<Client?> FindByClientIdAsync(string clientId);
    }
}
