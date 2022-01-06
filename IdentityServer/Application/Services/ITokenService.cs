namespace IdentityServer.Application
{
    public interface ITokenService
    {
        Task<Token> CreateIdentityTokenAsync(TokenRequest request);
        Task<Token> CreateAccessTokenAsync(TokenRequest request);
        Task<string> CreateSecurityTokenAsync(Token token);
    }
}
