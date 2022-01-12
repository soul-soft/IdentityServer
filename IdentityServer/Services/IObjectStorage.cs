namespace IdentityServer.Services
{
    public interface IObjectStorage
    {
        Task SetAsync(string key, object value, TimeSpan expiration);
        byte[] SerializeToUtf8Bytes(object obj);
    }
}
