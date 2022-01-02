using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientSecretValidationResult : ValidationResult
    {
        public Client Client { get; private set; }

        public ParsedSecret Secret { get; private set; }

        public ClientSecretValidationResult()
        {
            Client = null!;
            Secret = null!;
        }

        public ClientSecretValidationResult(Client client, ParsedSecret secret)
        {
            Client = client;
            Secret = secret;
        }
    }
}
