namespace IdentityServer.Validation
{
    public interface IRefreshTokenRequestValidator
    {
        Task ValidateAsync(RefreshTokenValidation context);
    }
}
