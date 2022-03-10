namespace IdentityServer.Validation
{
    public interface IClientCredentialsGrantValidator
    {
        Task ValidateAsync(ClientGrantValidationRequest context);
    }
}
