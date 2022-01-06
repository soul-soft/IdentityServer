using System.Collections.Specialized;
using IdentityServer.Models;

namespace IdentityServer.Application
{
    public class TokenRequestValidationRequest
    {
        public NameValueCollection Parameters { get; }

        public TokenRequestValidationRequest(NameValueCollection parameters)
        {
            Parameters = parameters;
        }
    }
}
