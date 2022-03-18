using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class IntrospectionGeneratorRequest
    {
        public ApiResource ApiResource { get; }
        public TokenValidationResult TokenValidationResult { get; }

        public IntrospectionGeneratorRequest(ApiResource apiResource, TokenValidationResult tokenValidationResult)
        {
            ApiResource = apiResource;
            TokenValidationResult = tokenValidationResult;
        }
    }
}
