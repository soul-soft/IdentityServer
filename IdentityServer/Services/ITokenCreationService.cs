namespace IdentityServer.Services
{
    public interface ITokenCreationService
    {
        Task<string> CreateTokenAsync(Token token);
    }
}
