namespace IdentityServer.Validation
{
    public interface IRefreshTokenRequestValidator
    {
        Task<RefreshTokenValidationResult> ValidateAsync(RefreshTokenValidationRequest request);
    }
}
