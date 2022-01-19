using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class GrantRequest
    {
        public IClient Client { get; }
        public string GrantType { get; }
        public ClientSecret ClientSecret { get; }
        public IEnumerable<string> Scopes { get; }
        public Resources Resources { get; }
        public IdentityServerOptions Options { get; }
        public NameValueCollection Raw { get; }

        public GrantRequest(
            IClient client,
            ClientSecret clientSecret,
            IdentityServerOptions options,
            IEnumerable<string> scopes,
            string grantType,
            Resources resources,
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
