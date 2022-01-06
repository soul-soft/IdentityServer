namespace IdentityServer.Application
{
    public interface ITokenCreationService
    {
        Task<string> CreateTokenAsync(Token token);
    }
}
