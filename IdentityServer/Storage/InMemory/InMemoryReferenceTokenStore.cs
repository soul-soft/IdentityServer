namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IPersistentStore _storage;

        public InMemoryReferenceTokenStore(IPersistentStore storage)
        {
            _storage = storage;
        }

        public async Task<ReferenceToken?> FindByIdAsync(string id)
        {
            var key = BuildStoreKey(id);
            return await _storage.GetAsync<ReferenceToken>(key);
        }

        public async Task AddAsync(ReferenceToken token)
        {
            var key = BuildStoreKey(token.Id);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private static string BuildStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:ReferenceToken:{id}";
        }
    }
}
