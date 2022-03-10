using System.Security.Claims;

namespace IdentityServer.Models
{
    public class UserInfoRequestContext
    {
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }

        public UserInfoRequestContext(Client client, ClaimsPrincipal subject)
        {
            Client = client;
            Subject = subject;
        }
    }
}
