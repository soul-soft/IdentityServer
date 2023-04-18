namespace IdentityServer.Endpoints
{
    internal class IntrospectionResponseGenerator : IIntrospectionResponseGenerator
    {
        public Task<IntrospectionGeneratorResponse> ProcessAsync(IntrospectionGeneratorRequest request)
        {
            var response = new Dictionary<string, object>
            {
                { "active", false }
            };
            if (!request.IsAuthentication)
            {
                return Task.FromResult(new IntrospectionGeneratorResponse(response));
            }
            var tokenScopes = request.Subject.Claims
                .Where(a => a.Type == JwtClaimTypes.Scope)
                .Select(s => s.Value);
            var apiResourceScopes = request.ApiResource.AllowedScopes;
            var allowScopes = tokenScopes.Where(a => apiResourceScopes.Contains(a));
            if (!allowScopes.Any())
            {
                return Task.FromResult(new IntrospectionGeneratorResponse(response));
            }
            var entity = request.Subject.Claims
                .Where(a => a.Type != JwtClaimTypes.Scope)
                .ToClaimsDictionary();
            response = new Dictionary<string, object>(entity)
            {
                { "active", true }
            };
            if (allowScopes.Count() == 1)
            {
                response.Add(JwtClaimTypes.Scope, allowScopes.First());
            }
            else
            {
                response.Add(JwtClaimTypes.Scope, allowScopes);
            }
            return Task.FromResult(new IntrospectionGeneratorResponse(response));
        }
    }
}
