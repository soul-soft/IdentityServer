namespace IdentityServer.Models
{
    public class SingInRequestContext
    {
        public string GrantType { get; }
        public Client Client { get; }
        public Resources Resources { get; }

        public SingInRequestContext(string grantType, Client client, Resources resources)
        {
            GrantType = grantType;
            Client = client;
            Resources = resources;
        }
    }
}
