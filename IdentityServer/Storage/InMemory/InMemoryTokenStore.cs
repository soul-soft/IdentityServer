namespace IdentityServer.Storage
{
    internal class InMemoryTokenStore : ITokenStore
    {
        private readonly ICache _storage;

        public InMemoryTokenStore(ICache storage)
        {
            _storage = storage;
        }

        public async Task<Token?> FindTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _storage.GetAsync<Token>(key);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var key = GenerateStoreKey(token.Id);
            await _storage.SetAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private static string GenerateStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:ReferenceToken:{id}";
        }
    }
}
