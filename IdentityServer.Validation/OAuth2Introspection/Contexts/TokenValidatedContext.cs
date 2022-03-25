using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenValidatedContext : ResultContext<OAuth2IntrospectionOptions>
    {
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();

        public TokenValidatedContext(HttpContext context, AuthenticationScheme scheme, OAuth2IntrospectionOptions options) : base(context, scheme, options)
        {

        }
    }
}
