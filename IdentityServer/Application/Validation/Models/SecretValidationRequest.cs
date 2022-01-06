using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class SecretValidationRequest
    {
        public ParsedCredential ParsedSecret { get; }
        public IEnumerable<ISecret> AllowedSecrets { get; }

        public SecretValidationRequest(IEnumerable<ISecret> allowedSecrets, ParsedCredential parsedSecret)
        {
            AllowedSecrets = allowedSecrets;
            ParsedSecret = parsedSecret;
        }
    }
}
