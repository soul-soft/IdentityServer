namespace IdentityServer.Application
{
    public interface ITokenService
    {
        Task<Token> CreateIdentityTokenAsync(TokenCreationRequest request);
        Task<Token> CreateAccessTokenAsync(TokenCreationRequest request);
        Task<string> CreateSecurityTokenAsync(Token token);
    }
}
