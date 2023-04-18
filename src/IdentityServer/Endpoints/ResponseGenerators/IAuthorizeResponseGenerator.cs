namespace IdentityServer.Endpoints
{
    public interface IAuthorizeResponseGenerator
    {
        Task<string> GenerateAsync(AuthorizeGeneratorRequest request);
    }
}
