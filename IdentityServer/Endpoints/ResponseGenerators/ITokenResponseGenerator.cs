namespace IdentityServer.ResponseGenerators
{
    public interface ITokenResponseGenerator
    {
        Task<TokenResponse> ProcessAsync(TokenRequest request);
    }
}
