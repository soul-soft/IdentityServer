using System.Security.Claims;

namespace IdentityServer.Models
{
    public class TokenGeneratorRequest
    {
        public string GrantType { get; }
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }
        public Resources Resources { get; }
        public TokenGeneratorRequest(string grantType, ClaimsPrincipal subject, Client client, Resources resources)
        {
            GrantType = grantType;
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
