namespace IdentityServer.Application
{
    public interface IClientCredentialsGrantValidator
    {
        Task<GrantValidationResult> ValidateAsync(ClientCredentialsGrantRequest request);
    }
}
