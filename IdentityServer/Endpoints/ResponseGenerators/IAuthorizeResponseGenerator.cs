namespace IdentityServer.Endpoints
{
    public interface IAuthorizeResponseGenerator
    {
        Task<AuthorizeGeneratorResponse> ProcessAsync(AuthorizeGeneratorRequest request);
    }
}
