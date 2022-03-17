using System.Security.Claims;

namespace IdentityServer.Models
{
    public class IsActiveContext
    {
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }

        public IsActiveContext(Client client, ClaimsPrincipal subject)
        {
            Client = client;
            Subject = subject;
        }
    }
}
