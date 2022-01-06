using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClaimsRequest
    {
        public IClient Client { get; }
        public IEnumerable<string> RequestedClaimTypes { get; set; }

        public ClaimsRequest(IClient client, IEnumerable<string> requestedClaimTypes)
        {
            Client = client;
            RequestedClaimTypes = requestedClaimTypes;
        }
    }
}
