namespace IdentityServer.Endpoints
{
    public interface ITokenResponseGenerator
    {
        Task<TokenGeneratorResponse> ProcessAsync(TokenGeneratorRequest request);
    }
}
