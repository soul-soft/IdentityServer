using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ExtensionGrantRequest : GrantRequest
    {
        public ExtensionGrantRequest(Client client) : base(client)
        {

        }
    }
}
