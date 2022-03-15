namespace IdentityServer.Endpoints
{
    internal class IntrospectionResponseGenerator : IIntrospectionResponseGenerator
    {
        public Task<IntrospectionResponse> ProcessAsync(IntrospectionRequest request)
        {
            var claims = request.Claims.ToClaimsDictionary();
            var entities = new Dictionary<string, object>(claims);
            entities.Add("active", !request.IsActive);
            return Task.FromResult(new IntrospectionResponse(entities));
        }
    }
}
