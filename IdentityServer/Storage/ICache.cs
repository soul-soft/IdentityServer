namespace IdentityServer.Storage
{
    public interface ICache
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan timeSpan);
        Task RevomeAsync(string key);
    }
}
