namespace IdentityServer.Endpoints
{
    public interface IIntrospectionResponseGenerator
    {
        Task<IntrospectionGeneratorResponse> GenerateAsync(IntrospectionGeneratorRequest request);
    }
}
