using IdentityServer.Models;

namespace IdentityServer.Storage
{
    public interface IClientStore
    {
        Task<IClient?> GetAsync(string clientId);
    }
}
