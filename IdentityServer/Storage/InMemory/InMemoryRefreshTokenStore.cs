namespace IdentityServer.Storage
{
    internal class InMemoryRefreshTokenStore : IRefreshTokenStore
    {
        private readonly IObjectStorage _storage;

        public InMemoryRefreshTokenStore(IObjectStorage storage)
        {
            _storage = storage;
        }

        public async Task SaveAsync(IRefreshToken token)
        {
            var key = CreateKey(token);
            await _storage.SetAsync(key, token, TimeSpan.FromSeconds(token.Lifetime));
        }

        private string CreateKey(IRefreshToken token)
        {
            return $"{Constants.IdentityServerName}:RefreshToken:{token.Id}";
        }
    }
}
