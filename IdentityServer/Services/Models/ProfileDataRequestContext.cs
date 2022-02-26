namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public IEnumerable<string> ClaimTypes { get; }

        public ProfileDataRequestContext(string caller, Client client, IEnumerable<string> claimTypes)
        {
            Caller = caller;
            Client = client;
            ClaimTypes = claimTypes;
        }
    }
}
