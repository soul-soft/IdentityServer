using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ResourceOwnerPasswordGrantRequest
    {
        public Client Client { get; }
        public string UserName { get; }
        public string Password { get; }
        public ResourceOwnerPasswordGrantRequest(Client client, string userName, string password)
        {
            Client = client;
            UserName = userName;
            Password = password;
        }
    }
}
