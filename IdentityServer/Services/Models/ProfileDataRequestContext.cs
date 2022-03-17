namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public Resources Resources { get; }
        public IEnumerable<string> ClaimTypes => Resources.ClaimTypes;

        public ProfileDataRequestContext(string caller, Client client, Resources resources)
        {
            Caller = caller;
            Client = client;
            Resources = resources;
        }
    }
}
