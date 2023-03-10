using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileClaimsRequest
    {
        public Client Client { get; }
        public Resources Resources { get; }
        public ClaimsPrincipal Subject { get; }
        public string AuthenticationType { get;}
        public IEnumerable<string> ClaimTypes => Resources.ClaimTypes;

        public ProfileClaimsRequest(string authenticationType, ClaimsPrincipal subject, Client client, Resources resources)
        {
            AuthenticationType = authenticationType;
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
