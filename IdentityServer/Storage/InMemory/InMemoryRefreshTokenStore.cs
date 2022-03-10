namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IObjectStore _storage;
       
        public InMemoryRefreshTokenStore(IObjectStore storage)
        {
            _storage = storage;
        }

        public async Task<RefreshToken?> FindRefreshTokenAsync(string id)
        {
            var key = BuildStoreKey(id);
            return await _storage.GetAsync<RefreshToken>(key);
        }

        public async Task StoreRefreshTokenAsync(RefreshToken refreshToken)
        {
            var key = BuildStoreKey(refreshToken.Id);
            await _storage.SetAsync(key, refreshToken, TimeSpan.FromSeconds(refreshToken.Lifetime));
        }

        public async Task RevomeRefreshTokenAsync(RefreshToken token)
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
