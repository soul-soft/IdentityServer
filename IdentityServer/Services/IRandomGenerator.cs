namespace IdentityServer.Services
{
    public interface IRandomGenerator
    {
        Task<string> GenerateAsync();
    }
}
