namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(Token token);
        Task<string> CreateRefreshTokenAsync(Token token, int lifetime);
        Task<Token> CreateAccessTokenAsync(TokenRequest request);
        Task<Token> CreateIdentityTokenAsync(TokenRequest request);
    }
}
