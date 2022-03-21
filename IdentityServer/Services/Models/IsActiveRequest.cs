using System.Security.Claims;

namespace IdentityServer.Models
{
    public class IsActiveRequest
    {
        public string Caller { get; }
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }

        public IsActiveRequest(string caller, Client client, ClaimsPrincipal subject)
        {
            Caller = caller;
            Client = client;
            Subject = subject;
        }
    }
}
