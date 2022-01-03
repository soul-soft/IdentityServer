using IdentityServer.Models;

namespace IdentityServer.Application
{
    public abstract class GrantRequest
    {
        public Client Client { get; }

        public GrantRequest(Client client)
        {
            Client = client;
        }
    }
}
