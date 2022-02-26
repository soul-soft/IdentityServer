namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IPersistentStore _storage;

        public InMemoryRefreshTokenStore(IPersistentStore storage)
        {
            _storage = storage;
        }

        public async Task<IRefreshToken?> GetAsync(string id)
        {
            var key = CreateKey(id);
            return await _storage.GetAsync<IRefreshToken>(key);
        }

        public async Task SaveAsync(IRefreshToken token)
        {
            var key = CreateKey(token.Id);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        public async Task RevomeAsync(IRefreshToken token)
        {
            var key = CreateKey(token.Id);
            await _storage.RevomeAsync(key);
        }

        private string CreateKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:RefreshToken:{id}";
        }
    }
}
