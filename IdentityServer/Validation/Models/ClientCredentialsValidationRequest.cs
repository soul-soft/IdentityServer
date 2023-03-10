using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class ClientCredentialsValidationRequest : GrantValidationRequest
    {
        public ClientCredentialsValidationRequest(GrantValidationRequest request) 
            : base(request.Client, request.GrantType, request.Resources, request.Form, request.Options)
        {

        }
    }
}
