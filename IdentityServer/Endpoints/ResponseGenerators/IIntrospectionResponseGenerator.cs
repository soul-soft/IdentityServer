namespace IdentityServer.Endpoints
{
    public interface IIntrospectionResponseGenerator
    {
        Task<IntrospectionGeneratorResponse> ProcessAsync(IntrospectionGeneratorRequest request);
    }
}
