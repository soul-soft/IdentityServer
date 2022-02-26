namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IPersistentStore _storage;

        public InMemoryRefreshTokenStore(IPersistentStore storage)
        {
            _storage = storage;
        }

        public async Task<RefreshToken?> FindByIdAsync(string id)
        {
            var key = BuildStoreKey(id);
            return await _storage.GetAsync<RefreshToken>(key);
        }

        public async Task AddAsync(RefreshToken token)
        {
            var key = BuildStoreKey(token.Id);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        public async Task RevomeAsync(RefreshToken token)
        {
            var key = BuildStoreKey(token.Id);
            await _storage.RevomeAsync(key);
        }

        private static string BuildStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:RefreshToken:{id}";
        }
    }
}
