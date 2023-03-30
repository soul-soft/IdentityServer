using System.Security.Claims;

namespace IdentityServer.Models
{
    public class AccessTokenClaimsRequest
    {
        public Client Client { get; }
        public string GrantType { get; }
        public Resources Resources { get; }
        public ClaimsPrincipal Subject { get; }
        public IEnumerable<string> ClaimTypes => Resources.AllowedClaimTypes;

        public AccessTokenClaimsRequest(string grantType, ClaimsPrincipal subject, Client client, Resources resources)
        {
            GrantType = grantType;
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
