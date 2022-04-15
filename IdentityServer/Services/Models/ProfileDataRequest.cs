using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequest
    {
        public Client Client { get; }
        public Resources Resources { get; }
        public ClaimsPrincipal Subject { get; }
        public IEnumerable<string> ClaimTypes => Resources.ClaimTypes;

        public ProfileDataRequest(ClaimsPrincipal subject, Client client, Resources resources)
        {
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
