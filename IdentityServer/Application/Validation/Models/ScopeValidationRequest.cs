using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ScopeValidationRequest
    {
        public IClient Client { get; }
        public string Scopes { get; }
        public ScopeValidationRequest(IClient client, string scopes)
        {
            Client = client;
            Scopes = scopes;
        }
    }
}
