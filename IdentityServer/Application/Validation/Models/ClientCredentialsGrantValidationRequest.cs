using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientCredentialsGrantValidationRequest
    {
        public IClient Client { get; }

        public ClientCredentialsGrantValidationRequest(IClient client)
        {
            Client = client;
        }
    }
}
