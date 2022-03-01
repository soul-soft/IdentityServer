using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ClaimDataRequestContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public IEnumerable<string> AllowedClaimTypes { get; }

        public ClaimDataRequestContext(string caller, Client client, IEnumerable<string> claimTypes)
        {
            Caller = caller;
            Client = client;
            AllowedClaimTypes = claimTypes;
        }
    }
}
