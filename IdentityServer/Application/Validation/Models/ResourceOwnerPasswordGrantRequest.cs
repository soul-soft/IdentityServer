using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ResourceOwnerPasswordGrantRequest
    {
        public IClient Client { get; }
        public string UserName { get; }
        public string Password { get; }
        public ResourceOwnerPasswordGrantRequest(IClient client, string userName, string password)
        {
            Client = client;
            UserName = userName;
            Password = password;
        }
    }
}
