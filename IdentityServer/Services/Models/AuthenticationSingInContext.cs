namespace IdentityServer.Models
{
    public class AuthenticationSingInContext
    {
        public Client Client { get; }
        public string GrantType { get; }
        public Resources Resources { get; }

        public AuthenticationSingInContext(string grantType, Client client, Resources resources)
        {
            Client = client;
            GrantType = grantType;
            Resources = resources;
        }
    }
}
