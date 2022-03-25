using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace IdentityServer.Validation
{
    public class MessageReceivedContext : ResultContext<OAuth2IntrospectionOptions>
    {
        public string? Token
        {
            get;
            set;
        }

        public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, OAuth2IntrospectionOptions options) 
            : base(context, scheme, options)
        {
        }
    }
}
