namespace IdentityServer.Validation
{
    public interface IRefreshTokenGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(RefreshTokenGrantValidationContext context);
    }
}
