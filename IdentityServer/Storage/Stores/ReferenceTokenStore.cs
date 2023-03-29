namespace IdentityServer.Storage
{
    internal class ReferenceTokenStore : IReferenceTokenStore
    {
        private readonly ICache _cache;
        private readonly IdentityServerOptions _options;

        public ReferenceTokenStore(
            ICache storage,
            IdentityServerOptions options)
        {
            _cache = storage;
            _options = options;
        }

        public async Task<ReferenceToken?> FindTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _cache.GetAsync<ReferenceToken>(key);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var key = GenerateStoreKey(token.Id);
            var span = TimeSpan.FromSeconds(token.Lifetime);
            await _cache.SetAsync(key, token, span);
        }

        private string GenerateStoreKey(string id)
        {
            return $"{_options.StorageKeyPrefix}:Token:{id}";
        }
    }
}
