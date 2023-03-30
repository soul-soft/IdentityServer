namespace IdentityServer.Validation
{
    public interface IRefreshTokenRequestValidator
    {
        Task<GrantValidationResult> ValidateAsync(RefreshTokenValidationRequest request);
    }
}
