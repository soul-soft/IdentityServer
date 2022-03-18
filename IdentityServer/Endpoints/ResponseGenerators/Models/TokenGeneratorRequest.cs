using System.Security.Claims;

namespace IdentityServer.Models
{
    public class TokenGeneratorRequest
    {
        public string GrantType { get; }
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }
        public Resources Resources { get; }
        public IdentityServerOptions Options { get; }

        public TokenGeneratorRequest(string grantType, ClaimsPrincipal subject, Client client, Resources resources, IdentityServerOptions options)
        {
            GrantType = grantType;
            Subject = subject;
            Client = client;
            Options = options;
            Resources = resources;
        }
    }
}
