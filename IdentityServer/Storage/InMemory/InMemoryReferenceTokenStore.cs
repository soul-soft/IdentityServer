namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IPersistentStore _storage;

        public InMemoryReferenceTokenStore(IPersistentStore storage)
        {
            _storage = storage;
        }

        public async Task<IReferenceToken?> FindReferenceTokenByIdAsync(string id)
        {
            var key = CreateKey(id);
            return await _storage.GetAsync<ReferenceToken>(key);
        }

        public async Task SaveAsync(IReferenceToken token)
        {
            var key = CreateKey(token.Id);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private string CreateKey(string id)
        {
            return $"{Constants.IdentityServerName}:ReferenceToken:{id}";
        }
    }
}
