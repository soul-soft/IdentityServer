using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class IntrospectionRequest
    {
        public bool IsError { get; }
        public ApiResource ApiResource { get; }
        public IEnumerable<Claim> Claims { get; }

        public IntrospectionRequest(bool isActive, ApiResource apiResource, IEnumerable<Claim> claims)
        {
            IsError = isActive;
            Claims = claims;
            ApiResource = apiResource;
        }
    }
}
