using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenCreationRequest
    {
        public string Issuer { get; internal set; }
        public IClient Client { get; internal set; }
        public string? SessionId { get; set; }
        public int Expires { get; set; }
        public TokenCreationRequest(string issuer, IClient client)
        {
            Issuer = issuer;
            Client = client;
        }
    }
}
