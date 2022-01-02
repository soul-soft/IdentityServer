namespace IdentityServer.Application
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(TokenRequestValidationResult validationResult);
    }
}
