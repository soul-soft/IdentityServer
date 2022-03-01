namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IIdGenerator _idGenerator;
        private readonly ICache _storage;

        public InMemoryReferenceTokenStore(
            IIdGenerator idGenerator,
            ICache storage)
        {
            _storage = storage;
            _idGenerator = idGenerator;
        }

        public async Task<ReferenceToken?> FindReferenceTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _storage.GetAsync<ReferenceToken>(key);
        }

        public async Task<string> StoreReferenceTokenAsync(Token token)
        {
            var id = await _idGenerator.GenerateAsync();
            var key = GenerateStoreKey(id);
            await _storage.SetAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
            return key;
        }

        private static string GenerateStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:ReferenceToken:{id}";
        }
    }
}
