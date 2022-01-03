using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IClientStore
    {
        Task<IClient?> FindClientByIdAsync(string clientId);
    }
}
