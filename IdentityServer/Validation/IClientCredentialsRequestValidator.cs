namespace IdentityServer.Validation
{
    public interface IClientCredentialsRequestValidator
    {
        Task ValidateAsync(ClientCredentialsRequestValidation context);
    }
}
