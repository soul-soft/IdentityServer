namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<TokenValidationResult> ValidateAccessTokenAsync(string token);
    }
}
