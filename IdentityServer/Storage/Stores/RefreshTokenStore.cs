namespace IdentityServer.Storage
{
    internal class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly ICache _storage;
        private readonly IdentityServerOptions _options;

        public RefreshTokenStore(
            ICache storage, 
            IdentityServerOptions options)
        {
            _storage = storage;
            _options = options;
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

        private string BuildStoreKey(string id)
        {
            return $"{_options.DistributedStorageKeyPrefix}:RefreshToken:{id}";
        }
    }
}
