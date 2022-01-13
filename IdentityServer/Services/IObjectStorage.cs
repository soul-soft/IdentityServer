namespace IdentityServer.Services
{
    public interface IObjectStorage
    {
        byte[] SerializeToUtf8Bytes(object obj);
        Task SaveAsync(string key, object value, TimeSpan expiration);
    }
}
