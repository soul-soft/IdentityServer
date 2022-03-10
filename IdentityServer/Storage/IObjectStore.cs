namespace IdentityServer.Storage
{
    public interface IObjectStore
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync(string key, object value, TimeSpan timeSpan);
        Task RevomeAsync(string key);
    }
}
