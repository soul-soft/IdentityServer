namespace IdentityServer.Validation
{
    public interface IRefreshTokenGrantValidator
    {
        Task ValidateAsync(RefreshTokenGrantValidationRequest context);
    }
}
