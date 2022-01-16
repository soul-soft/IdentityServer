namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<IAccessToken> CreateAccessTokenAsync(TokenRequest request);
        Task<IAccessToken> CreateIdentityTokenAsync(TokenRequest request);
        Task<string> CreateSecurityTokenAsync(IAccessToken request);
    }
}
