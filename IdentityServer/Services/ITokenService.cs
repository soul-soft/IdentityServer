namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(Token token);
        Task<string> CreateSecurityRefreshTokenAsync(Token token, int lifetime);
        Task<Token> CreateAccessTokenAsync(ValidatedTokenRequest request);
        Task<Token> CreateIdentityTokenAsync(ValidatedTokenRequest request);
    }
}
