namespace IdentityServer.Endpoints
{
    public interface ITokenGenerator
    {
        Task<TokenResponse> ProcessAsync(ValidatedTokenRequest request);
    }
}
