using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequest
    {
        public string Caller { get; }
        public Client Client { get; }
        public Resources Resources { get; }
        public ClaimsPrincipal Subject { get; }
        public IEnumerable<string> ClaimTypes => Resources.ClaimTypes;

        public ProfileDataRequest(string caller, ClaimsPrincipal subject, Client client, Resources resources)
        {
            Caller = caller;
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
