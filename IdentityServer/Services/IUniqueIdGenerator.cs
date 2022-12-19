namespace IdentityServer.Services
{
    public interface IUniqueIdGenerator
    {
        Task<string> GenerateAsync(int length = 32);
    }
}
