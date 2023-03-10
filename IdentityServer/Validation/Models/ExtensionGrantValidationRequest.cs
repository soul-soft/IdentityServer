using System.Collections.Specialized;

namespace IdentityServer.Validation
{
    public class ExtensionGrantValidationRequest : GrantValidationRequest
    {
        public ExtensionGrantValidationRequest(GrantValidationRequest request)
            : base(request.Client, request.GrantType, request.Resources, request.Form, request.Options)
        {

        }
    }
}
