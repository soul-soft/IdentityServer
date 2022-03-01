namespace IdentityServer.Services
{
    public interface IHandleGenerator
    {
        Task<string> GenerateAsync(int length = 32);
    }
}
