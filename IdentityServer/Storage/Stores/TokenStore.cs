namespace IdentityServer.Storage
{
    internal class TokenStore : ITokenStore
    {
        private readonly ICache _cache;
        private readonly IdentityServerOptions _options;

        public TokenStore(
            ICache storage,
            IdentityServerOptions options)
        {
            _cache = storage;
            _options = options;
        }

        public async Task<Token?> FindTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _cache.GetAsync<Token>(key);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var key = GenerateStoreKey(token.Id);
            var span = token.Expiration - token.IssuedAt;
            await _cache.SetAsync(key, token, span);
        }

        private string GenerateStoreKey(string id)
        {
            return $"{_options.DistributedStorageKeyPrefix}:ReferenceToken:{id}";
        }
    }
}
