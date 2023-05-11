namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<TokenValidationResult> ValidateIdentityTokenAsync(string token);
        Task<TokenValidationResult> ValidateAccessTokenAsync(string token);
    }
}
