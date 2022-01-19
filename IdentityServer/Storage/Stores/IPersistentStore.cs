namespace IdentityServer.Storage
{
    public interface IPersistentStore
    {
        Task<T?> GetAsync<T>(string key);
        Task SaveAsync(string key, object value, TimeSpan timeSpan);
        Task RevomeAsync(string key);
    }
}
