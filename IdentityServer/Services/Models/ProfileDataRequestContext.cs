using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public Resources Resources { get; }
        public IEnumerable<string> ClaimTypes { get; }

        public ProfileDataRequestContext(string caller, Client client, Resources resources, IEnumerable<string> claimTypes)
        {
            Caller = caller;
            Client = client;
            Resources = resources;
            ClaimTypes = claimTypes;
        }
    }
}
