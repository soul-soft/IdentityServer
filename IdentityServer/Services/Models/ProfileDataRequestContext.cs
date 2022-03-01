using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ProfileDataRequestContext
    {
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }

        public ProfileDataRequestContext(Client client, ClaimsPrincipal subject)
        {
            Client = client;
            Subject = subject;
        }
    }
}
