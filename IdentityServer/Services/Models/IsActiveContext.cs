using System.Security.Claims;

namespace IdentityServer.Models
{
    public class IsActiveContext
    {
        public string Caller { get; }
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }
       
        public IsActiveContext(string caller, Client client, ClaimsPrincipal subject)
        {
            Client = client;
            Caller = caller;
            Subject = subject;
        }
    }
}
