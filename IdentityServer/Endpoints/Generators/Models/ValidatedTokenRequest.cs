using System.Security.Claims;

namespace IdentityServer.Models
{
    public class ValidatedTokenRequest
    {
        public string? Nonce { get; }
        public Client Client { get; }
        public ResourceCollection Resources { get; }
        public string? SessionId { get; set; }
        public string? Description { get; set; }
        public ClaimsPrincipal Subject { get; set; }
        public string? GrantType { get; set; }
        public IEnumerable<string> Scopes { get; set; } = new HashSet<string>();
        public ValidatedTokenRequest(ClaimsPrincipal subject, Client client, ResourceCollection resources)
        {
            Subject = subject;
            Client = client;
            Resources = resources;
        }
    }
}
