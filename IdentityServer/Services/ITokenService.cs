namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(Token token);
        Task<string> CreateRefreshTokenAsync(Token token, int lifetime);
        Task<Token> CreateAccessTokenAsync(TokenValidatedRequest request);
        Task<Token> CreateIdentityTokenAsync(TokenValidatedRequest request);
    }
}
