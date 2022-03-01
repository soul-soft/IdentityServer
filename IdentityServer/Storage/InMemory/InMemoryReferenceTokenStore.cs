namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly ICache _storage;

        public InMemoryReferenceTokenStore(ICache storage)
        {
            _storage = storage;
        }

        public async Task<ReferenceToken?> FindReferenceTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _storage.GetAsync<ReferenceToken>(key);
        }

        public async Task StoreReferenceTokenAsync(Token token)
        {
            var key = GenerateStoreKey(token.Id);
            await _storage.SetAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private static string GenerateStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:ReferenceToken:{id}";
        }
    }
}
