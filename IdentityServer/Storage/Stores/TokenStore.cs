namespace IdentityServer.Storage
{
    internal class TokenStore : ITokenStore
    {
        private readonly ICacheStore _cache;
        private readonly IdentityServerOptions _options;

        public TokenStore(ICacheStore cache, IdentityServerOptions options)
        {
            _cache = cache;
            _options = options;
        }

        public async Task<Token?> FindTokenAsync(string token)
        {
            var key = BuildStoreKey(token);
            return await _cache.GetAsync<Token>(key);
        }

        public async Task SaveTokenAsync(Token token)
        {
            var key = BuildStoreKey(token.Id);
            var span = TimeSpan.FromSeconds(token.Lifetime);
            await _cache.SaveAsync(key, token, span);
        }

        public async Task RevomeTokenAsync(Token token)
        {
            var key = BuildStoreKey(token.Id);
            await _cache.RevomeAsync(key);
        }

        private string BuildStoreKey(string id)
        {
            return $"{_options.StorageKeyPrefix}:Token:{id}";
        }
    }
}
