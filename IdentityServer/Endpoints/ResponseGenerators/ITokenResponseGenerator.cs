namespace IdentityServer.Endpoints
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(ValidatedTokenRequest request);
    }
}
