namespace IdentityServer.Validation
{
    public interface IClientGrantValidator
    {
        Task ValidateAsync(ClientGrantValidationRequest context);
    }
}
