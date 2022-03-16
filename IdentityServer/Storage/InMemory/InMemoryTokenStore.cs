namespace IdentityServer.Storage
{
    internal class InMemoryTokenStore : ITokenStore
    {
        private readonly IObjectStore _storage;

        public InMemoryTokenStore(IObjectStore storage)
        {
            _storage = storage;
        }

        public async Task<Token?> FindTokenAsync(string id)
        {
            var key = GenerateStoreKey(id);
            return await _storage.GetAsync<Token>(key);
        }

        public async Task StoreTokenAsync(Token token)
        {
            var key = GenerateStoreKey(token.JwtId);
            var span = token.Expiration - token.IssuedAt;
            await _storage.SetAsync(key, token, span);
        }

        private static string GenerateStoreKey(string id)
        {
            return $"{Constants.IdentityServerProvider}:ReferenceToken:{id}";
        }
    }
}
