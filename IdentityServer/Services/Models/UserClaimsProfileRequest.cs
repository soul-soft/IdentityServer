namespace IdentityServer.Models
{
    public class UserClaimsProfileRequest
    {
        public IClient Client { get; }
        public Resources Resources { get; }
        public string GrantType { get; }

        public UserClaimsProfileRequest(IClient client, Resources resources, string grantType)
        {
            Client = client;
            Resources = resources;
            GrantType = grantType;
        }
    }
}
