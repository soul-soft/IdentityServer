using IdentityServer.Models;

namespace IdentityServer.Storage.InMemory
{
    public class InMemoryClientStore : IClientStore
    {
        private readonly IEnumerable<Client> _clients;
      
        public InMemoryClientStore(IEnumerable<Client> clients)
        {
            _clients = clients;
        }

        public Task<Client?> FindClientByIdAsync(string clientId)
        {
            var client = _clients
                .Where(a => a.ClientId == clientId)
                .FirstOrDefault();

            return Task.FromResult(client);
        }
    }
}
