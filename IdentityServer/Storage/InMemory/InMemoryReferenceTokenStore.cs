namespace IdentityServer.Storage
{
    internal class InMemoryReferenceTokenStore : IReferenceTokenStore
    {
        private readonly IObjectStorage _storage;

        public InMemoryReferenceTokenStore(IObjectStorage storage)
        {
            _storage = storage;
        }

        public async Task SaveAsync(IReferenceToken token)
        {
            var key = CreateKey(token);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private string CreateKey(IReferenceToken token)
        {
            return $"{Constants.IdentityServerName}:ReferenceToken:{token.Id}";
        }
    }
}
