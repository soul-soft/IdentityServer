using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ValidatedTokenRequest
    {
        public string GrantType { get; }
        public Client Client { get; }
        public ResourceCollection Resources { get; }
        public IdentityServerOptions Options { get; }

        public ValidatedTokenRequest(string grantType, Client client, ResourceCollection resources, IdentityServerOptions options)
        {
            GrantType = grantType;
            Client = client;
            Options = options;
            Resources = resources;
        }
    }
}
