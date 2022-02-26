namespace IdentityServer.Services
{
    public interface ITokenService
    {        
        Task<string> CreateSecurityTokenAsync(AccessToken request);
        Task<AccessToken> CreateAccessTokenAsync(ValidatedTokenRequest request);
        Task<AccessToken> CreateIdentityTokenAsync(ValidatedTokenRequest request);
    }
}
