using System.Collections.Specialized;
using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class AuthorizeGeneratorRequest
    {
        public NameValueCollection Body { get; }
        public ClaimsPrincipal Subject { get; }
        public Client Client { get; }
        public Resources Resources { get; }

        public AuthorizeGeneratorRequest(NameValueCollection body, Client client, Resources resources, ClaimsPrincipal subject)
        {
            Body = body;
            Client = client;
            Resources = resources;
            Subject = subject;
        }
    }
}
