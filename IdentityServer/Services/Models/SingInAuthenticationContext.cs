using System.Security.Claims;

namespace IdentityServer.Models
{
    public class SingInAuthenticationContext
    {
        public Client Client { get; }
        public Resources Resources { get; }
        public IEnumerable<Claim> Claims { get; }
        public string AuthenticationType { get; }

        public SingInAuthenticationContext(Client client, Resources resources,string authenticationType, IEnumerable<Claim> claims)
        {
            Client = client;
            Resources = resources;
            Claims = claims;
            AuthenticationType = authenticationType;
        }
    }
}
