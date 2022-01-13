namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IObjectStorage _storage;

        public InMemoryRefreshTokenStore(IObjectStorage storage)
        {
            _storage = storage;
        }

        public async Task<IRefreshToken?> FindRefreshTokenByIdAsync(string id)
        {
            var key = CreateKey(id);
            return await _storage.GetAsync<RefreshToken>(key);
        }

        public async Task SaveAsync(IRefreshToken token)
        {
            var key = CreateKey(token.Id);
            await _storage.SaveAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private string CreateKey(string id)
        {
            return $"{Constants.IdentityServerName}:RefreshToken:{id}";
        }
    }
}
