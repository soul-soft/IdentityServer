namespace IdentityServer.Validation
{
    public interface IClientCredentialsRequestValidator
    {
        Task ValidateAsync(ClientCredentialsValidation context);
    }
}
