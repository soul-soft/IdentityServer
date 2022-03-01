namespace IdentityServer.Services
{
    public interface IIdGenerator
    {
        Task<string> GenerateAsync(int length = 32);
    }
}
