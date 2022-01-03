using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ResourceValidationRequest
    {
        public Client Client { get; }
        public IEnumerable<string> Scopes { get; }
        public ResourceValidationRequest(Client client, IEnumerable<string> scopes)
        {
            Client = client;
            Scopes = scopes;
        }
    }
}
