using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenRequestValidationResult : ValidationResult
    {
        public Client Client { get; private set; }
        public string Scopes { get; private set; }
        public IEnumerable<Resource> Resources { get; private set; }
      
    }
}
