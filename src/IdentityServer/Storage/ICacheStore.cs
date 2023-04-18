namespace IdentityServer.Storage
{
    public interface ICacheStore
    {
        Task<T?> GetAsync<T>(string key);
        Task SaveAsync(string key, object value, TimeSpan timeSpan);
        Task RevomeAsync(string key);
        Task RefreshAsync(string key);
    }
}
