using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class IntrospectionRequest
    {
        public bool IsActive { get; }
        public ApiResource ApiResource { get; }
        public IEnumerable<Claim> Claims { get; }

        public IntrospectionRequest(bool isActive, ApiResource apiResource, IEnumerable<Claim> claims)
        {
            IsActive = isActive;
            ApiResource = apiResource;
            Claims = claims;
        }
    }
}
