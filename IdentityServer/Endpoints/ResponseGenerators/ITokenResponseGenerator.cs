namespace IdentityServer.Endpoints
{
    public interface ITokenResponseGenerator
    {
        Task<TokenGeneratorResponse> CreateTokenAsync(TokenGeneratorRequest request);
    }
}
