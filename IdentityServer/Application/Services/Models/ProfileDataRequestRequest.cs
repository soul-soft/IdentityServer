using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ProfileDataRequestRequest
    {
        public ProfileDataRequestCaller Caller { get; }
        public IClient Client { get; }
        public IEnumerable<string> RequestedClaimTypes { get; }

        public ProfileDataRequestRequest(ProfileDataRequestCaller caller, IClient client, IEnumerable<string> requestedClaimTypes)
        {
            Caller = caller;
            Client = client;
            RequestedClaimTypes = requestedClaimTypes;
        }
    }
}
