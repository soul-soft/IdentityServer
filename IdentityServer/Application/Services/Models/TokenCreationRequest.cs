using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenCreationRequest
    {
        public string Issuer { get; internal set; }
        public Client Client { get; internal set; }
        public string? SessionId { get; set; }
        public string Description { get; internal set; }
        public int AccessTokenLifetime { get;  set; }
        public AccessTokenType AccessTokenType { get;  set; }
        public Resources Resources { get;  set; }
    }
}
