namespace IdentityServer.Storage
{
    internal class InMemoryClientStore : IClientStore
    {
        private readonly IEnumerable<Client> _clients;

        public InMemoryClientStore(IEnumerable<Client> clients)
        {
            _clients = clients;
        }

        public Task<Client?> FindByClientIdAsync(string clientId)
        {
            var client = _clients
                .Where(a => a.ClientId == clientId)
                .Where(a => a.Enabled)
                .FirstOrDefault();
            return Task.FromResult(client);
        }
    }
}
