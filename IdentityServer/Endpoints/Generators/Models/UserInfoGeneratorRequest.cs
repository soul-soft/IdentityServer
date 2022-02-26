using System.Security.Claims;

namespace IdentityServer.Endpoints
{
    public class UserInfoGeneratorRequest
    {
        public Client Client { get; }
        public ClaimsPrincipal Subject { get; }
        public ResourceCollection Resources { get; }

        public UserInfoGeneratorRequest(Client client, ClaimsPrincipal subject,  ResourceCollection resources)
        {
            Client = client;
            Subject = subject;
            Resources = resources;
        }
    }
}
