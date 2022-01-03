using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientCredentialsGrantRequest
        : GrantRequest
    {

        public ClientCredentialsGrantRequest(Client client)
            : base(client)
        {

        }
    }
}
