namespace IdentityServer.Validation
{
    public interface ITokenValidator
    {
        Task<TokenValidationResult> ValidateAsync(string token);
    }
}
