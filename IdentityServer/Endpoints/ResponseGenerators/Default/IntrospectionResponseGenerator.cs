namespace IdentityServer.Endpoints
{
    internal class IntrospectionResponseGenerator : IIntrospectionResponseGenerator
    {
        public Task<IntrospectionGeneratorResponse> ProcessAsync(IntrospectionGeneratorRequest request)
        {
            var errorResponse = new Dictionary<string, object>
            {
                { "active", false }
            };
            if (request.TokenValidationResult.IsError)
            {
                return Task.FromResult(new IntrospectionGeneratorResponse(errorResponse));
            }
            var tokenScopes = request.TokenValidationResult.Claims
                .Where(a => a.Type == JwtClaimTypes.Scope)
                .Select(s => s.Value);
            var apiResourceScopes = request.ApiResource.Scopes;
            var allowScopes = tokenScopes.Where(a => apiResourceScopes.Contains(a));
            if (!allowScopes.Any())
            {
                return Task.FromResult(new IntrospectionGeneratorResponse(errorResponse));
            }
            var entities = request.TokenValidationResult.Claims
                .Where(a => a.Type != JwtClaimTypes.Scope)
                .ToClaimsDictionary();
            var response = new Dictionary<string, object>(entities)
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
