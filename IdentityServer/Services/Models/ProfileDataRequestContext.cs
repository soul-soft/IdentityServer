using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public ResourceCollection Resources { get; }
        public IEnumerable<string> ClaimTypes { get; }

        public ProfileDataRequestContext(string caller, Client client, ResourceCollection resources, IEnumerable<string> claimTypes)
        {
            Caller = caller;
            Client = client;
            Resources = resources;
            ClaimTypes = claimTypes;
        }
    }
}
