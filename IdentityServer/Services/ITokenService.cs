namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<IAccessToken> CreateAccessTokenAsync(ValidatedTokenRequest request);
        Task<IAccessToken> CreateIdentityTokenAsync(ValidatedTokenRequest request);
        Task<string> CreateSecurityTokenAsync(IAccessToken request);
    }
}
