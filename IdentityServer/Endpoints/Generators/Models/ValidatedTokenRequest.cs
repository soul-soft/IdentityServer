using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ValidatedTokenRequest
    {
        public string? Nonce { get; }
        public Client Client { get; }
        public string? GrantType { get; set; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public ClaimsPrincipal Subject { get; set; }
        public ResourceCollection Resources { get; }
        public IdentityServerOptions Options { get; }
        public IEnumerable<string> Scopes { get; set; } = new HashSet<string>();
       
        public ValidatedTokenRequest(IdentityServerOptions options, ClaimsPrincipal subject, Client client, ResourceCollection resources)
        {
            Options = options;
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
