using System.Collections.Specialized;
using System.Security.Claims;

namespace IdentityServer.Validation
{
    public class TokenRequestValidation
    {
        public Client Client { get; }
        public string GrantType { get; set; }
        public ClaimsPrincipal Subject { get; }
        public Resources Resources { get; }
        public NameValueCollection Body { get; }
        public IdentityServerOptions Options { get; }

        public TokenRequestValidation(
            Client client,
            string grantType,
            Resources resources,
            ClaimsPrincipal subject,
            NameValueCollection body,
            IdentityServerOptions options)
        {
            Client = client;
            GrantType = grantType;
            Subject = subject;
            Options = options;
            Resources = resources;
            Body = body;
        }
    }
}
