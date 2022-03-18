using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class GrantValidationRequest
    {
        public Client Client { get; }
        public string GrantType { get; set; }
        public Resources Resources { get; }
        public NameValueCollection Body { get; }
        public IdentityServerOptions Options { get; }

        internal GrantValidationRequest(
            Client client,
            string grantType,
            Resources resources,
            NameValueCollection body,
            IdentityServerOptions options)
        {
            Client = client;
            Options = options;
            GrantType = grantType;
            Resources = resources;
            Body = body;
        }
    }
}
