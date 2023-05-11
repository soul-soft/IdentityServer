namespace IdentityServer.Endpoints
{
    public interface ITokenResponseGenerator
    {
        Task<TokenGeneratorResponse> GenerateAsync(TokenGeneratorRequest request);
    }
}
