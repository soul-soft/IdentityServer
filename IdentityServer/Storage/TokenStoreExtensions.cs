namespace IdentityServer.Storage
{
    public static class TokenStoreExtensions
    {
        public static async Task<Token?> FindAccessTokenAsync(this ITokenStore store, string token)
        {
            var result = await store.FindTokenAsync(token);
            if (result == null)
            {
                return default;
            }
            if (result.Type != TokenTypes.AccessToken)
            {
                return default;
            }
            return result;
        }

        public static async Task<Token?> FindRefreshTokenAsync(this ITokenStore store, string token)
        {
            var result = await store.FindTokenAsync(token);
            if (result == null)
            {
                return default;
            }
            if (result.Type != TokenTypes.RefreshToken)
            {
                return default;
            }
            return result;
        }
    }
}
