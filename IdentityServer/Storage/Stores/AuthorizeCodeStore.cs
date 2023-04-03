namespace IdentityServer.Storage
{
    internal class AuthorizeCodeStore : IAuthorizationCodeStore
    {
        private readonly ICacheStore _cache;
        private readonly IdentityServerOptions _options;

        public AuthorizeCodeStore(
            ICacheStore storage,
            IdentityServerOptions options)
        {
            _cache = storage;
            _options = options;
        }

        public async Task<AuthorizationCode?> FindAuthorizationCodeAsync(string id)
        {
            var key = BuildKey(id);
            return await _cache.GetAsync<AuthorizationCode>(key);
        }

        public async Task RevomeAuthorizationCodeAsync(AuthorizationCode code)
        {
            var key = BuildKey(code.Id);
            await _cache.RevomeAsync(key);
        }

        public async Task StoreAuthorizationCodeAsync(AuthorizationCode Code)
        {
            var key = BuildKey(Code.Id);
            await _cache.SaveAsync(key, Code, TimeSpan.FromSeconds(Code.Lifetime));
        }


        private string BuildKey(string id)
        {
            return $"{_options.StorageKeyPrefix}:AuthorizeCode:{id}";
        }
    }
}
