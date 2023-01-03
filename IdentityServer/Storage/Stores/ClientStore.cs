namespace IdentityServer.Storage
{
    internal class ClientStore : IClientStore
    {
        private readonly IEnumerable<Client> _clients;

        public ClientStore(IEnumerable<Client> clients)
        {
            _clients = clients;
        }

        public Task<Client?> FindByClientIdAsync(string clientId)
        {
            var client = _clients
                .Where(a => a.ClientId == clientId)
                .FirstOrDefault();

            return Task.FromResult(client);
        }
    }
}
