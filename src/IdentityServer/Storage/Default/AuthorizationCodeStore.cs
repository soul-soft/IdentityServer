namespace IdentityServer.Storage
{
    internal class AuthorizationCodeStore : IAuthorizationCodeStore
    {
        private readonly ICacheStore _cache;
        private readonly IdentityServerOptions _options;

        public AuthorizationCodeStore(
            ICacheStore storage,
            IdentityServerOptions options)
        {
            _cache = storage;
            _options = options;
        }

        public async Task<AuthorizationCode?> FindAuthorizationCodeAsync(string code)
        {
            var key = BuildKey(code);
            return await _cache.GetAsync<AuthorizationCode>(key);
        }

        public async Task RevomeAuthorizationCodeAsync(AuthorizationCode code)
        {
            var key = BuildKey(code.Code);
            await _cache.RevomeAsync(key);
        }

        public async Task SaveAuthorizationCodeAsync(AuthorizationCode code)
        {
            var key = BuildKey(code.Code);
            await _cache.SaveAsync(key, code, TimeSpan.FromSeconds(code.Lifetime));
        }


        private string BuildKey(string id)
        {
            return $"{_options.StorageKeyPrefix}:AuthorizeCode:{id}";
        }
    }
}
