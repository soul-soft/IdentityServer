using System.Security.Claims;

namespace IdentityServer.Models
{
    public class SingInAuthenticationContext
    {
        public Client Client { get; }
        public Resources Resources { get; }
        public ClaimsPrincipal Subject { get; }
        public string AuthenticationType { get; }

        public SingInAuthenticationContext(Client client, ClaimsPrincipal subject, Resources resources, string authenticationType)
        {
            Client = client;
            Subject = subject;
            Resources = resources;
            AuthenticationType = authenticationType;
        }
    }
}
