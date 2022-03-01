namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(Token request);
        Task<Token> CreateAccessTokenAsync(ValidatedTokenRequest request);
        Task<Token> CreateIdentityTokenAsync(ValidatedTokenRequest request);
    }
}
