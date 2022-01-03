using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class SecretValidationRequest
    {
        public ParsedSecret ParsedSecret { get; }
        public IEnumerable<ISecret> AllowedSecrets { get; }

        public SecretValidationRequest(IEnumerable<ISecret> allowedSecrets, ParsedSecret parsedSecret)
        {
            AllowedSecrets = allowedSecrets;
            ParsedSecret = parsedSecret;
        }
    }
}
