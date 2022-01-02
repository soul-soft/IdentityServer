using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IClientStore
    {
        Task<Client?> FindClientByIdAsync(string clientId);
    }
}
