using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class TokenValidationRequest
    {
        public Client Client { get; }
        public string GrantType { get; }
        public ClientCredentials ClientSecret { get; }
        public IEnumerable<string> Scopes { get; }
        public ResourceCollection Resources { get; }
        public IdentityServerOptions Options { get; }
        public NameValueCollection Raw { get; }

        public TokenValidationRequest(
            Client client,
            ClientCredentials clientSecret,
            IdentityServerOptions options,
            IEnumerable<string> scopes,
            string grantType,
            ResourceCollection resources,
            NameValueCollection raw)
        {
            Client = client;
            ClientSecret = clientSecret;
            Options = options;
            Scopes = scopes;
            Resources = resources;
            GrantType = grantType;
            Raw = raw;
        }
    }
}
