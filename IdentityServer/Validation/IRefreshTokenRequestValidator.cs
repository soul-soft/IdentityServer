namespace IdentityServer.Validation
{
    public interface IRefreshTokenRequestValidator
    {
        Task ValidateAsync(RefreshTokenRequestValidation context);
    }
}
