using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientSecretValidationResult : ValidationResult
    {
        public Client Client { get; }

        public ParsedSecret ParsedSecret { get; }

        public ClientSecretValidationResult()
        {
            Client = null!;
            ParsedSecret = null!;
        }

        public ClientSecretValidationResult(Client client, ParsedSecret secret)
        {
            Client = client;
            ParsedSecret = secret;
        }
    }
}
