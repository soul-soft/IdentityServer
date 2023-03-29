using IdentityServer.Models;

namespace IdentityServer.Storage
{
    internal class AuthorizeCodeStore : IAuthorizeCodeStore
    {
        private readonly ICache _cache;
        private readonly IdentityServerOptions _options;

        public AuthorizeCodeStore(
            ICache storage,
            IdentityServerOptions options)
        {
            _cache = storage;
            _options = options;
        }

        public async Task<AuthorizeCode?> FindByAuthorizeCodeAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _cache.GetAsync<AuthorizeCode>(key);
        }

        public async Task RevomeAuthorizeCodeAsync(string id)
        {
            var key = GenerateStoreKey(id);
            await _cache.RevomeAsync(key);
        }

        public async Task StoreAuthorizeCodeAsync(AuthorizeCode Code)
        {
            var key = GenerateStoreKey(Code.Id);
            await _cache.SetAsync(key, Code, TimeSpan.FromSeconds(Code.Lifetime));
        }


        private string GenerateStoreKey(string id)
        {
            return $"{_options.StorageKeyPrefix}:AuthorizeCode:{id}";
        }
    }
}
