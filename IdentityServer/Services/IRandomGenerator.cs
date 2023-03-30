namespace IdentityServer.Services
{
    public interface IRandomGenerator
    {
        Task<string> GenerateAsync(int length = 32);
    }
}
