using System.Collections.Specialized;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenRequestValidationRequest
    {
        public IClient Client { get; }
        public NameValueCollection Parameters { get; }

        public TokenRequestValidationRequest(IClient client, NameValueCollection parameters)
        {
            Client = client;
            Parameters = parameters;
        }
    }
}
