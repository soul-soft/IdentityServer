using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientValidationRequest
    {
        public IClient Client { get; set; }

        public ClientValidationRequest(IClient client)
        {
            Client = client;
        }
    }
}
