namespace IdentityServer.Endpoints
{
    public interface IAuthorizeResponseGenerator
    {
        Task<AuthorizationCode> GenerateAsync(AuthorizeGeneratorRequest request);
    }
}
