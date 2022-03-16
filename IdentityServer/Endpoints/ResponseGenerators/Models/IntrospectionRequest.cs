using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class IntrospectionRequest
    {
        public ApiResource ApiResource { get; }
        public TokenValidationResult TokenValidationResult { get; }

        public IntrospectionRequest(ApiResource apiResource, TokenValidationResult tokenValidationResult)
        {
            ApiResource = apiResource;
            TokenValidationResult = tokenValidationResult;
        }
    }
}
