namespace IdentityServer.Endpoints
{
    internal class IntrospectionResponseGenerator : IIntrospectionResponseGenerator
    {
        public Task<IntrospectionResponse> ProcessAsync(IntrospectionRequest request)
        {
            var errorResponse = new Dictionary<string, object>();
            errorResponse.Add("active", false);
            if (request.IsError)
            {
                return Task.FromResult(new IntrospectionResponse(errorResponse));
            }

            var tokenScopes = request.Claims
                .Where(a => a.Type == JwtClaimTypes.Scope)
                .Select(s => s.Value);
            var apiResourceScopes = request.ApiResource.Scopes;
            var allowScopes = tokenScopes.Where(a => apiResourceScopes.Contains(a));
            if (!allowScopes.Any())
            {
                return Task.FromResult(new IntrospectionResponse(errorResponse));
            }

            var entities = request.Claims
                .Where(a => a.Type != JwtClaimTypes.Scope)
                .ToClaimsDictionary();
            var response = new Dictionary<string, object>(entities);
            response.Add("active", true);
            if (allowScopes.Count() == 1)
            {
                response.Add(JwtClaimTypes.Scope, allowScopes.First());
            }
            else
            {
                response.Add(JwtClaimTypes.Scope, allowScopes.First());
            }
            return Task.FromResult(new IntrospectionResponse(response));
        }
    }
}
