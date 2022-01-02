using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class ClientSecretValidationContext 
        : ValidatorContext
    {

        public Client Client { get; }
        public ParsedSecret ParsedSecret { get; }

        public ClientSecretValidationContext(Client client, ParsedSecret parsedSecret)
        {
            Client = client;
            ParsedSecret = parsedSecret;
        }
    }
}
