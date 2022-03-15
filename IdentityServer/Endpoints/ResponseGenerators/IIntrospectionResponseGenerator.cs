namespace IdentityServer.Endpoints
{
    public interface IIntrospectionResponseGenerator
    {
        Task<IntrospectionResponse> ProcessAsync(IntrospectionRequest request);
    }
}
