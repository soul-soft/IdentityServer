namespace IdentityServer.Storage
{
    public static class TokenStoreExtensions
    {
        public static async Task<Token?> FindAccessTokenAsync(this ITokenStore store, string token)
        {
            var result = await store.FindTokenAsync(token);
            if (result == null)
            {
                return null;
            }
            if (result.Type != TokenTypes.AccessToken)
            {
                return null;
            }
            return result;
        }

        public static async Task<Token?> FindRefreshTokenAsync(this ITokenStore store, string token)
        {
            var result = await store.FindTokenAsync(token);
            if (result == null)
            {
                return null;
            }
            if (result.Type != TokenTypes.RefreshToken)
            {
                return null;
            }
            return result;
        }
    }
}
